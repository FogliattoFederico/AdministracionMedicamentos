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
                m.nombre AS nombre_medicamento,
                m.presentacion
            FROM medicamentos_pacientes mp
            JOIN medicamentos m ON mp.medicamento_id = m.id
            WHERE mp.paciente_id = @PacienteId AND mp.activo = 1", conn);

        cmd.Parameters.AddWithValue("@PacienteId", pacienteId);
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new MedicamentoPaciente
            {
                Id = (int)reader["id"],
                PacienteId = (int)reader["paciente_id"],
                MedicamentoId = (int)reader["medicamento_id"],
                Dosis = reader["dosis"].ToString()!,
                Frecuencia = reader["frecuencia"].ToString()!,
                Horarios = reader["horarios"].ToString(),
                Via = reader["via"].ToString(),
                Indicacion = reader["indicacion"].ToString(),
                FechaInicio = (DateTime)reader["fecha_inicio"],
                FechaFin = reader["fecha_fin"] == DBNull.Value ? null : (DateTime?)reader["fecha_fin"],
                Activo = (bool)reader["activo"],
                PrescriptoPor = reader["prescripto_por"].ToString(),
                NombreMedicamento = reader["nombre_medicamento"].ToString(),
                Presentacion = reader["presentacion"].ToString()
            });
        }

        return lista;
    }

    // AGREGAR
    public void Agregar(MedicamentoPaciente mp)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            INSERT INTO medicamentos_pacientes 
                (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, fecha_fin, prescripto_por)
            VALUES 
                (@PacienteId, @MedicamentoId, @Dosis, @Frecuencia, @Horarios, @Via, @Indicacion, @FechaInicio, @FechaFin, @PrescriptoPor)", conn);

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

        cmd.ExecuteNonQuery();
    }

    // MODIFICAR
    public void Modificar(MedicamentoPaciente mp)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            UPDATE medicamentos_pacientes SET
                dosis          = @Dosis,
                frecuencia     = @Frecuencia,
                horarios       = @Horarios,
                via            = @Via,
                indicacion     = @Indicacion,
                fecha_inicio   = @FechaInicio,
                fecha_fin      = @FechaFin,
                prescripto_por = @PrescriptoPor
            WHERE id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", mp.Id);
        cmd.Parameters.AddWithValue("@Dosis", mp.Dosis);
        cmd.Parameters.AddWithValue("@Frecuencia", mp.Frecuencia);
        cmd.Parameters.AddWithValue("@Horarios", (object?)mp.Horarios ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Via", (object?)mp.Via ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Indicacion", (object?)mp.Indicacion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@FechaInicio", mp.FechaInicio);
        cmd.Parameters.AddWithValue("@FechaFin", (object?)mp.FechaFin ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@PrescriptoPor", (object?)mp.PrescriptoPor ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    // ELIMINAR (baja lógica)
    public void Eliminar(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(
            "UPDATE medicamentos_pacientes SET activo = 0 WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }

    public MedicamentoPaciente? ObtenerPorId(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
        SELECT 
            mp.id, mp.paciente_id, mp.medicamento_id,
            mp.dosis, mp.frecuencia, mp.horarios, mp.via,
            mp.indicacion, mp.fecha_inicio, mp.fecha_fin,
            mp.activo, mp.prescripto_por
        FROM medicamentos_pacientes mp
        WHERE mp.id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new MedicamentoPaciente
            {
                Id = (int)reader["id"],
                PacienteId = (int)reader["paciente_id"],
                MedicamentoId = (int)reader["medicamento_id"],
                Dosis = reader["dosis"].ToString()!,
                Frecuencia = reader["frecuencia"].ToString()!,
                Horarios = reader["horarios"].ToString(),
                Via = reader["via"].ToString(),
                Indicacion = reader["indicacion"].ToString(),
                FechaInicio = (DateTime)reader["fecha_inicio"],
                FechaFin = reader["fecha_fin"] == DBNull.Value ? null : (DateTime?)reader["fecha_fin"],
                Activo = (bool)reader["activo"],
                PrescriptoPor = reader["prescripto_por"].ToString()
            };
        }

        return null;
    }
}