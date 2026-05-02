namespace Geriatrico.Models.Negocio;

using Microsoft.Data.SqlClient;

public class DashboardNegocio
{
    private readonly string _connectionString;

    public DashboardNegocio(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int TotalPacientes()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        var cmd = new SqlCommand("SELECT COUNT(*) FROM pacientes WHERE activo = 1", conn);
        return (int)cmd.ExecuteScalar();
    }

    public int TotalMedicamentos()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        var cmd = new SqlCommand("SELECT COUNT(*) FROM medicamentos", conn);
        return (int)cmd.ExecuteScalar();
    }

    public int AdministracionesHoyTomadas()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        var cmd = new SqlCommand(@"
            SELECT COUNT(*) FROM administraciones 
            WHERE fecha = @Hoy AND tomado = 1", conn);
        cmd.Parameters.AddWithValue("@Hoy", DateTime.Today);
        return (int)cmd.ExecuteScalar();
    }

    public int AdministracionesHoyPendientes()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        var cmd = new SqlCommand(@"
            SELECT COUNT(*) FROM administraciones 
            WHERE fecha = @Hoy AND tomado = 0", conn);
        cmd.Parameters.AddWithValue("@Hoy", DateTime.Today);
        return (int)cmd.ExecuteScalar();
    }
}