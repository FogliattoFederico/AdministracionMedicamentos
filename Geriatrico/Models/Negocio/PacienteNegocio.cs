namespace Geriatrico.Models.Negocio;

using Microsoft.Data.SqlClient;
using Geriatrico.Models.Dominio;

public class PacienteNegocio
{
    private readonly string _connectionString;

    public PacienteNegocio(string connectionString)
    {
        _connectionString = connectionString;
    }

    // LISTAR
    public List<Paciente> Listar()
    {
        var lista = new List<Paciente>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT * FROM pacientes WHERE activo = 1", conn);
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Paciente
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Apellido = reader["apellido"].ToString()!,
                Dni = reader["dni"].ToString()!,
                FechaNac = (DateTime)reader["fecha_nac"],
                Habitacion = reader["habitacion"].ToString(),
                ObraSocial = reader["obra_social"].ToString(),
                NroAfiliado = reader["nro_afiliado"].ToString(),
                ContactoNombre = reader["contacto_nombre"].ToString(),
                ContactoTel = reader["contacto_tel"].ToString(),
                Activo = (bool)reader["activo"]
            });
        }

        return lista;
    }

    // OBTENER POR ID
    public Paciente? ObtenerPorId(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT * FROM pacientes WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Paciente
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Apellido = reader["apellido"].ToString()!,
                Dni = reader["dni"].ToString()!,
                FechaNac = (DateTime)reader["fecha_nac"],
                Habitacion = reader["habitacion"].ToString(),
                ObraSocial = reader["obra_social"].ToString(),
                NroAfiliado = reader["nro_afiliado"].ToString(),
                ContactoNombre = reader["contacto_nombre"].ToString(),
                ContactoTel = reader["contacto_tel"].ToString(),
                Activo = (bool)reader["activo"]
            };
        }

        return null;
    }

    // AGREGAR
    public void Agregar(Paciente p)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            INSERT INTO pacientes (nombre, apellido, dni, fecha_nac, habitacion, obra_social, nro_afiliado, contacto_nombre, contacto_tel)
            VALUES (@Nombre, @Apellido, @Dni, @FechaNac, @Habitacion, @ObraSocial, @NroAfiliado, @ContactoNombre, @ContactoTel)", conn);

        cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@Apellido", p.Apellido);
        cmd.Parameters.AddWithValue("@Dni", p.Dni);
        cmd.Parameters.AddWithValue("@FechaNac", p.FechaNac);
        cmd.Parameters.AddWithValue("@Habitacion", (object?)p.Habitacion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ObraSocial", (object?)p.ObraSocial ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@NroAfiliado", (object?)p.NroAfiliado ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ContactoNombre", (object?)p.ContactoNombre ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ContactoTel", (object?)p.ContactoTel ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    // MODIFICAR
    public void Modificar(Paciente p)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
            UPDATE pacientes SET
                nombre          = @Nombre,
                apellido        = @Apellido,
                dni             = @Dni,
                fecha_nac       = @FechaNac,
                habitacion      = @Habitacion,
                obra_social     = @ObraSocial,
                nro_afiliado    = @NroAfiliado,
                contacto_nombre = @ContactoNombre,
                contacto_tel    = @ContactoTel
            WHERE id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", p.Id);
        cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@Apellido", p.Apellido);
        cmd.Parameters.AddWithValue("@Dni", p.Dni);
        cmd.Parameters.AddWithValue("@FechaNac", p.FechaNac);
        cmd.Parameters.AddWithValue("@Habitacion", (object?)p.Habitacion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ObraSocial", (object?)p.ObraSocial ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@NroAfiliado", (object?)p.NroAfiliado ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ContactoNombre", (object?)p.ContactoNombre ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ContactoTel", (object?)p.ContactoTel ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    // ELIMINAR (baja lógica)
    public void Eliminar(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("UPDATE pacientes SET activo = 0 WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }

    public List<Paciente> Buscar(string termino)
    {
        var lista = new List<Paciente>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
        SELECT * FROM pacientes 
        WHERE activo = 1 AND (
            nombre    LIKE @Termino OR
            apellido  LIKE @Termino OR
            dni       LIKE @Termino
        )
        ORDER BY apellido", conn);

        cmd.Parameters.AddWithValue("@Termino", "%" + termino + "%");
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Paciente
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Apellido = reader["apellido"].ToString()!,
                Dni = reader["dni"].ToString()!,
                FechaNac = (DateTime)reader["fecha_nac"],
                Habitacion = reader["habitacion"].ToString(),
                ObraSocial = reader["obra_social"].ToString(),
                NroAfiliado = reader["nro_afiliado"].ToString(),
                ContactoNombre = reader["contacto_nombre"].ToString(),
                ContactoTel = reader["contacto_tel"].ToString(),
                Activo = (bool)reader["activo"]
            });
        }

        return lista;
    }

    public List<Paciente> UltimosAgregados(int cantidad = 5)
    {
        var lista = new List<Paciente>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand($@"
        SELECT TOP {cantidad} * FROM pacientes 
        WHERE activo = 1 
        ORDER BY created_at DESC", conn);

        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Paciente
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Apellido = reader["apellido"].ToString()!,
                Dni = reader["dni"].ToString()!,
                FechaNac = (DateTime)reader["fecha_nac"],
                Habitacion = reader["habitacion"].ToString(),
                ObraSocial = reader["obra_social"].ToString(),
                Activo = (bool)reader["activo"]
            });
        }

        return lista;
    }

    public List<Paciente> ListarPaginado(int pagina, int porPagina, string? buscar = null)
    {
        var lista = new List<Paciente>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var filtro = string.IsNullOrWhiteSpace(buscar)
            ? ""
            : "AND (nombre LIKE @Buscar OR apellido LIKE @Buscar OR dni LIKE @Buscar)";

        var cmd = new SqlCommand($@"
        SELECT * FROM pacientes
        WHERE activo = 1 {filtro}
        ORDER BY apellido
        OFFSET @Offset ROWS FETCH NEXT @PorPagina ROWS ONLY", conn);

        cmd.Parameters.AddWithValue("@Offset", (pagina - 1) * porPagina);
        cmd.Parameters.AddWithValue("@PorPagina", porPagina);

        if (!string.IsNullOrWhiteSpace(buscar))
            cmd.Parameters.AddWithValue("@Buscar", "%" + buscar + "%");

        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Paciente
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Apellido = reader["apellido"].ToString()!,
                Dni = reader["dni"].ToString()!,
                FechaNac = (DateTime)reader["fecha_nac"],
                Habitacion = reader["habitacion"].ToString(),
                ObraSocial = reader["obra_social"].ToString(),
                NroAfiliado = reader["nro_afiliado"].ToString(),
                ContactoNombre = reader["contacto_nombre"].ToString(),
                ContactoTel = reader["contacto_tel"].ToString(),
                Activo = (bool)reader["activo"]
            });
        }

        return lista;
    }

    public int ContarPacientes(string? buscar = null)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var filtro = string.IsNullOrWhiteSpace(buscar)
            ? ""
            : "AND (nombre LIKE @Buscar OR apellido LIKE @Buscar OR dni LIKE @Buscar)";

        var sql = $"SELECT COUNT(*) FROM pacientes WHERE activo = 1 {filtro}";

        

        var cmd = new SqlCommand(sql, conn);

        if (!string.IsNullOrWhiteSpace(buscar))
            cmd.Parameters.AddWithValue("@Buscar", "%" + buscar + "%");

        var resultado = (int)cmd.ExecuteScalar();

       

        return resultado;
    }
}