namespace Geriatrico.Models.Negocio;

using Microsoft.Data.SqlClient;
using Geriatrico.Models.Dominio;

public class MedicamentoNegocio
{
    private readonly string _connectionString;

    public MedicamentoNegocio(string connectionString)
    {
        _connectionString = connectionString;
    }

    // LISTAR
    public List<Medicamento> Listar()
    {
        var lista = new List<Medicamento>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT * FROM medicamentos ORDER BY nombre", conn);
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Medicamento
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                NombreGenerico = reader["nombre_generico"].ToString(),
                Presentacion = reader["presentacion"].ToString(),
                Laboratorio = reader["laboratorio"].ToString()
            });
        }

        return lista;
    }

    // OBTENER POR ID
    public Medicamento? ObtenerPorId(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT * FROM medicamentos WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Medicamento
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                NombreGenerico = reader["nombre_generico"].ToString(),
                Presentacion = reader["presentacion"].ToString(),
                Laboratorio = reader["laboratorio"].ToString()
            };
        }

        return null;
    }

    // AGREGAR
    public void Agregar(Medicamento m)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            INSERT INTO medicamentos (nombre, nombre_generico, presentacion, laboratorio)
            VALUES (@Nombre, @NombreGenerico, @Presentacion, @Laboratorio)", conn);

        cmd.Parameters.AddWithValue("@Nombre", m.Nombre);
        cmd.Parameters.AddWithValue("@NombreGenerico", (object?)m.NombreGenerico ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Presentacion", (object?)m.Presentacion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Laboratorio", (object?)m.Laboratorio ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    // MODIFICAR
    public void Modificar(Medicamento m)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            UPDATE medicamentos SET
                nombre          = @Nombre,
                nombre_generico = @NombreGenerico,
                presentacion    = @Presentacion,
                laboratorio     = @Laboratorio
            WHERE id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", m.Id);
        cmd.Parameters.AddWithValue("@Nombre", m.Nombre);
        cmd.Parameters.AddWithValue("@NombreGenerico", (object?)m.NombreGenerico ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Presentacion", (object?)m.Presentacion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Laboratorio", (object?)m.Laboratorio ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    // ELIMINAR
    public void Eliminar(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("DELETE FROM medicamentos WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }
}