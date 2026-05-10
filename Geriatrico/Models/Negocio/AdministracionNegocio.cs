namespace Geriatrico.Models.Negocio;

using Microsoft.Data.SqlClient;
using Geriatrico.Models.Dominio;

public class AdministracionNegocio
{
    private readonly string _connectionString;

    public AdministracionNegocio(string connectionString)
    {
        _connectionString = connectionString;
    }

    // LISTAR ADMINISTRACIONES DE UN PACIENTE POR FECHA
    public List<Administracion> ListarPorPacienteYFecha(int pacienteId, DateTime fecha)
    {
        var lista = new List<Administracion>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            SELECT 
                a.id, a.medicamento_pac_id, a.fecha, a.hora_programada,
                a.hora_administrada, a.administrado_por, a.tomado, a.observaciones,
                m.nombre AS nombre_medicamento,
                mp.dosis
            FROM administraciones a
            JOIN medicamentos_pacientes mp ON a.medicamento_pac_id = mp.id
            JOIN medicamentos m ON mp.medicamento_id = m.id
            WHERE mp.paciente_id = @PacienteId AND a.fecha = @Fecha
            ORDER BY a.hora_programada", conn);

        cmd.Parameters.AddWithValue("@PacienteId", pacienteId);
        cmd.Parameters.AddWithValue("@Fecha", fecha.Date);

        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Administracion
            {
                Id = (int)reader["id"],
                MedicamentoPacId = (int)reader["medicamento_pac_id"],
                Fecha = (DateTime)reader["fecha"],
                HoraProgramada = reader["hora_programada"].ToString()!,
                HoraAdministrada = reader["hora_administrada"] == DBNull.Value ? null : reader["hora_administrada"].ToString(),
                AdministradoPor = reader["administrado_por"] == DBNull.Value ? null : reader["administrado_por"].ToString(),
                Tomado = (bool)reader["tomado"],
                Observaciones = reader["observaciones"] == DBNull.Value ? null : reader["observaciones"].ToString(),
                NombreMedicamento = reader["nombre_medicamento"].ToString(),
                Dosis = reader["dosis"].ToString()
            });
        }

        return lista;
    }

    // GENERAR ADMINISTRACIONES DEL DIA
    public void GenerarDelDia(int pacienteId, DateTime fecha)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmdMeds = new SqlCommand(@"
            SELECT id, horarios, tipo_frecuencia, dias_semana, 
                   intervalo_dias, dia_mes, fecha_proxima_toma
            FROM medicamentos_pacientes
            WHERE paciente_id = @PacienteId AND activo = 1
            AND fecha_inicio <= @Fecha
            AND (fecha_fin IS NULL OR fecha_fin >= @Fecha)", conn);

        cmdMeds.Parameters.AddWithValue("@PacienteId", pacienteId);
        cmdMeds.Parameters.AddWithValue("@Fecha", fecha.Date);

        var reader = cmdMeds.ExecuteReader();

        var medicamentos = new List<(int Id, string Horarios, string TipoFrecuencia, string? DiasSemana, int? IntervaloDias, int? DiaMes, DateTime? FechaProximaToma)>();

        while (reader.Read())
        {
            medicamentos.Add((
                (int)reader["id"],
                reader["horarios"].ToString() ?? "08:00",
                reader["tipo_frecuencia"] == DBNull.Value ? "diaria" : reader["tipo_frecuencia"].ToString()!,
                reader["dias_semana"] == DBNull.Value ? null : reader["dias_semana"].ToString(),
                reader["intervalo_dias"] == DBNull.Value ? null : (int?)reader["intervalo_dias"],
                reader["dia_mes"] == DBNull.Value ? null : (int?)reader["dia_mes"],
                reader["fecha_proxima_toma"] == DBNull.Value ? null : (DateTime?)reader["fecha_proxima_toma"]
            ));
        }
        reader.Close();

        // Nombres de días en español
        var nombresDias = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Monday,    "Lunes" },
            { DayOfWeek.Tuesday,   "Martes" },
            { DayOfWeek.Wednesday, "Miércoles" },
            { DayOfWeek.Thursday,  "Jueves" },
            { DayOfWeek.Friday,    "Viernes" },
            { DayOfWeek.Saturday,  "Sábado" },
            { DayOfWeek.Sunday,    "Domingo" }
        };

        var diaHoy = nombresDias[fecha.DayOfWeek];

        foreach (var med in medicamentos)
        {
            var correspondeHoy = false;

            switch (med.TipoFrecuencia)
            {
                case "diaria":
                    correspondeHoy = true;
                    break;

                case "semanal":
                    if (!string.IsNullOrWhiteSpace(med.DiasSemana))
                    {
                        var dias = med.DiasSemana.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(d => d.Trim());
                        correspondeHoy = dias.Contains(diaHoy);
                    }
                    break;

                case "intervalo":
                    if (med.FechaProximaToma.HasValue)
                        correspondeHoy = med.FechaProximaToma.Value.Date == fecha.Date;
                    break;

                case "mensual":
                    if (med.DiaMes.HasValue)
                        correspondeHoy = med.DiaMes.Value == fecha.Day;
                    break;
            }

            if (!correspondeHoy) continue;

            var horarios = med.Horarios.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var horario in horarios)
            {
                var hora = horario.Trim();

                var cmdCheck = new SqlCommand(@"
                    SELECT COUNT(*) FROM administraciones
                    WHERE medicamento_pac_id = @MedId AND fecha = @Fecha AND hora_programada = @Hora", conn);

                cmdCheck.Parameters.AddWithValue("@MedId", med.Id);
                cmdCheck.Parameters.AddWithValue("@Fecha", fecha.Date);
                cmdCheck.Parameters.AddWithValue("@Hora", hora);

                var existe = (int)cmdCheck.ExecuteScalar() > 0;

                if (!existe)
                {
                    var cmdInsert = new SqlCommand(@"
                        INSERT INTO administraciones (medicamento_pac_id, fecha, hora_programada)
                        VALUES (@MedId, @Fecha, @Hora)", conn);

                    cmdInsert.Parameters.AddWithValue("@MedId", med.Id);
                    cmdInsert.Parameters.AddWithValue("@Fecha", fecha.Date);
                    cmdInsert.Parameters.AddWithValue("@Hora", hora);

                    cmdInsert.ExecuteNonQuery();

                    // Si es por intervalo, actualizar fecha proxima toma
                    if (med.TipoFrecuencia == "intervalo" && med.IntervaloDias.HasValue)
                    {
                        var nuevaFecha = fecha.Date.AddDays(med.IntervaloDias.Value);

                        var cmdUpdate = new SqlCommand(@"
                            UPDATE medicamentos_pacientes 
                            SET fecha_proxima_toma = @NuevaFecha 
                            WHERE id = @MedId", conn);

                        cmdUpdate.Parameters.AddWithValue("@NuevaFecha", nuevaFecha);
                        cmdUpdate.Parameters.AddWithValue("@MedId", med.Id);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    // REGISTRAR ADMINISTRACION
    public void Registrar(Administracion a)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            UPDATE administraciones SET
                tomado            = @Tomado,
                hora_administrada = @HoraAdministrada,
                administrado_por  = @AdministradoPor,
                observaciones     = @Observaciones
            WHERE id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", a.Id);
        cmd.Parameters.AddWithValue("@Tomado", a.Tomado);
        cmd.Parameters.AddWithValue("@HoraAdministrada", (object?)a.HoraAdministrada ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@AdministradoPor", (object?)a.AdministradoPor ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Observaciones", (object?)a.Observaciones ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }
}