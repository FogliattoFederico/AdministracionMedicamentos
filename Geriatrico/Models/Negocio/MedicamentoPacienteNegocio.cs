namespace Geriatrico.Models.Negocio;

using Microsoft.Data.SqlClient;
using Geriatrico.Models.Dominio;

public class MedicamentoPacienteNegocio
{
    private readonly string _connectionString;

    public MedicamentoPacienteNegocio(string connectionString)
    {
        _connectionString = connectionString;
    }

    // LISTAR MEDICAMENTOS DE UN PACIENTE
    public List<MedicamentoPaciente> ListarPorPaciente(int pacienteId)
    {
        var lista = new List<MedicamentoPaciente>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            SELECT 
                mp.id, mp.paciente_id, mp.medicamento_id,
                mp.dosis, mp.frecuencia, mp.horarios, mp.via,
                mp.indicacion, mp.fecha_inicio, mp.fecha_fin,
                mp.activo, mp.prescripto_por,
                mp.tipo_frecuencia, mp.dias_semana, mp.intervalo_dias,
                mp.dia_mes, mp.fecha_proxima_toma,
                m.nombre AS nombre_medicamento,
                m.presentacion
            FROM medicamentos_pacientes mp
            JOIN medicamentos m ON mp.medicamento_id = m.id
            WHERE mp.paciente_id = @PacienteId AND mp.activo = 1", conn);

        cmd.Parameters.AddWithValue("@PacienteId", pacienteId);
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(MapearMedicamentoPaciente(reader));
        }

        return lista;
    }

    // OBTENER POR ID
    public MedicamentoPaciente? ObtenerPorId(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            SELECT 
                mp.id, mp.paciente_id, mp.medicamento_id,
                mp.dosis, mp.frecuencia, mp.horarios, mp.via,
                mp.indicacion, mp.fecha_inicio, mp.fecha_fin,
                mp.activo, mp.prescripto_por,
                mp.tipo_frecuencia, mp.dias_semana, mp.intervalo_dias,
                mp.dia_mes, mp.fecha_proxima_toma,
                m.nombre AS nombre_medicamento,
                m.presentacion
            FROM medicamentos_pacientes mp
            JOIN medicamentos m ON mp.medicamento_id = m.id
            WHERE mp.id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        var reader = cmd.ExecuteReader();

        if (reader.Read())
            return MapearMedicamentoPaciente(reader);

        return null;
    }

    // AGREGAR
    public void Agregar(MedicamentoPaciente mp)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            INSERT INTO medicamentos_pacientes 
                (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, 
                 fecha_inicio, fecha_fin, prescripto_por, tipo_frecuencia, dias_semana, 
                 intervalo_dias, dia_mes, fecha_proxima_toma)
            VALUES 
                (@PacienteId, @MedicamentoId, @Dosis, @Frecuencia, @Horarios, @Via, @Indicacion,
                 @FechaInicio, @FechaFin, @PrescriptoPor, @TipoFrecuencia, @DiasSemana,
                 @IntervaloDias, @DiaMes, @FechaProximaToma)", conn);

        AgregarParametros(cmd, mp);
        cmd.ExecuteNonQuery();
    }

    // MODIFICAR
    public void Modificar(MedicamentoPaciente mp)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            UPDATE medicamentos_pacientes SET
                dosis              = @Dosis,
                frecuencia         = @Frecuencia,
                horarios           = @Horarios,
                via                = @Via,
                indicacion         = @Indicacion,
                fecha_inicio       = @FechaInicio,
                fecha_fin          = @FechaFin,
                prescripto_por     = @PrescriptoPor,
                tipo_frecuencia    = @TipoFrecuencia,
                dias_semana        = @DiasSemana,
                intervalo_dias     = @IntervaloDias,
                dia_mes            = @DiaMes,
                fecha_proxima_toma = @FechaProximaToma
            WHERE id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", mp.Id);
        AgregarParametros(cmd, mp);
        cmd.ExecuteNonQuery();
    }

    // ELIMINAR (baja lógica)
    public void Eliminar(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        // Borrar administraciones futuras no registradas
        var cmdAdmin = new SqlCommand(@"
        DELETE FROM administraciones 
        WHERE medicamento_pac_id = @Id 
        AND tomado = 0
        AND fecha >= CAST(GETDATE() AS DATE)", conn);
        cmdAdmin.Parameters.AddWithValue("@Id", id);
        cmdAdmin.ExecuteNonQuery();

        // Baja lógica del medicamento
        var cmd = new SqlCommand(
            "UPDATE medicamentos_pacientes SET activo = 0 WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    // ACTUALIZAR FECHA PROXIMA TOMA
    public void ActualizarFechaProximaToma(int id, DateTime fechaProximaToma)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(
            "UPDATE medicamentos_pacientes SET fecha_proxima_toma = @Fecha WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Fecha", fechaProximaToma);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    // METODOS PRIVADOS
    private MedicamentoPaciente MapearMedicamentoPaciente(SqlDataReader reader)
    {
        return new MedicamentoPaciente
        {
            Id = (int)reader["id"],
            PacienteId = (int)reader["paciente_id"],
            MedicamentoId = (int)reader["medicamento_id"],
            Dosis = reader["dosis"].ToString()!,
            Frecuencia = reader["frecuencia"].ToString()!,
            Horarios = reader["horarios"] == DBNull.Value ? null : reader["horarios"].ToString(),
            Via = reader["via"] == DBNull.Value ? null : reader["via"].ToString(),
            Indicacion = reader["indicacion"] == DBNull.Value ? null : reader["indicacion"].ToString(),
            FechaInicio = (DateTime)reader["fecha_inicio"],
            FechaFin = reader["fecha_fin"] == DBNull.Value ? null : (DateTime?)reader["fecha_fin"],
            Activo = (bool)reader["activo"],
            PrescriptoPor = reader["prescripto_por"] == DBNull.Value ? null : reader["prescripto_por"].ToString(),
            TipoFrecuencia = reader["tipo_frecuencia"] == DBNull.Value ? "diaria" : reader["tipo_frecuencia"].ToString()!,
            DiasSemana = reader["dias_semana"] == DBNull.Value ? null : reader["dias_semana"].ToString(),
            IntervaloDias = reader["intervalo_dias"] == DBNull.Value ? null : (int?)reader["intervalo_dias"],
            DiaMes = reader["dia_mes"] == DBNull.Value ? null : (int?)reader["dia_mes"],
            FechaProximaToma = reader["fecha_proxima_toma"] == DBNull.Value ? null : (DateTime?)reader["fecha_proxima_toma"],
            NombreMedicamento = reader["nombre_medicamento"] == DBNull.Value ? null : reader["nombre_medicamento"].ToString(),
            Presentacion = reader["presentacion"] == DBNull.Value ? null : reader["presentacion"].ToString()
        };
    }

    private void AgregarParametros(SqlCommand cmd, MedicamentoPaciente mp)
    {
        cmd.Parameters.AddWithValue("@PacienteId", mp.PacienteId);
        cmd.Parameters.AddWithValue("@MedicamentoId", mp.MedicamentoId);
        cmd.Parameters.AddWithValue("@Dosis", mp.Dosis);
        cmd.Parameters.AddWithValue("@Frecuencia", mp.Frecuencia);
        cmd.Parameters.AddWithValue("@Horarios", (object?)mp.Horarios ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Via", (object?)mp.Via ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Indicacion", (object?)mp.Indicacion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@FechaInicio", mp.FechaInicio);
        cmd.Parameters.AddWithValue("@FechaFin", (object?)mp.FechaFin ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@PrescriptoPor", (object?)mp.PrescriptoPor ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TipoFrecuencia", mp.TipoFrecuencia);
        cmd.Parameters.AddWithValue("@DiasSemana", (object?)mp.DiasSemana ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IntervaloDias", (object?)mp.IntervaloDias ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@DiaMes", (object?)mp.DiaMes ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@FechaProximaToma", (object?)mp.FechaProximaToma ?? DBNull.Value);
    }
}