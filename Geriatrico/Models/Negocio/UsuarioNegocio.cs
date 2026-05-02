namespace Geriatrico.Models.Negocio;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Geriatrico.Models.Dominio;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

public class UsuarioNegocio
{
    private readonly string _connectionString;

    public UsuarioNegocio(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Usuario? Login(string email, string password)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
        SELECT * FROM usuarios 
        WHERE email = @Email AND activo = 1", conn);

        cmd.Parameters.AddWithValue("@Email", email);

        var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            var hashGuardado = reader["password"].ToString()!;

            if (VerificarPassword(password, hashGuardado))
            {
                return new Usuario
                {
                    Id = (int)reader["id"],
                    Nombre = reader["nombre"].ToString()!,
                    Email = reader["email"].ToString()!,
                    Activo = (bool)reader["activo"],
                    Rol = reader["rol"].ToString()!
                };
            }
        }

        return null;
    }

    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32
        );

        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
    }

    public static bool VerificarPassword(string password, string hashGuardado)
    {
        var partes = hashGuardado.Split(':');
        if (partes.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(partes[0]);
        byte[] hashOriginal = Convert.FromBase64String(partes[1]);

        byte[] hashNuevo = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32
        );

        return CryptographicOperations.FixedTimeEquals(hashOriginal, hashNuevo);
    }

    public List<Usuario> Listar()
    {
        var lista = new List<Usuario>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT * FROM usuarios WHERE activo = 1 ORDER BY nombre", conn);
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Usuario
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Email = reader["email"].ToString()!,
                Rol = reader["rol"].ToString()!,
                Activo = (bool)reader["activo"]
            });
        }

        return lista;
    }

    public void Agregar(Usuario u)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
        INSERT INTO usuarios (nombre, email, password, rol)
        VALUES (@Nombre, @Email, @Password, @Rol)", conn);

        cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
        cmd.Parameters.AddWithValue("@Email", u.Email);
        cmd.Parameters.AddWithValue("@Password", u.Password);
        cmd.Parameters.AddWithValue("@Rol", u.Rol);

        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("UPDATE usuarios SET activo = 0 WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }

    public Usuario? ObtenerPorEmail(string email)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT * FROM usuarios WHERE email = @Email AND activo = 1", conn);
        cmd.Parameters.AddWithValue("@Email", email);

        var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Usuario
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Email = reader["email"].ToString()!,
                Rol = reader["rol"].ToString()!,
                Activo = (bool)reader["activo"]
            };
        }

        return null;
    }

    public void ModificarPerfil(Usuario u)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(@"
        UPDATE usuarios SET
            nombre   = @Nombre,
            email    = @Email,
            password = @Password
        WHERE id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", u.Id);
        cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
        cmd.Parameters.AddWithValue("@Email", u.Email);
        cmd.Parameters.AddWithValue("@Password", u.Password);

        cmd.ExecuteNonQuery();
    }

    public Usuario? ObtenerPorId(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT * FROM usuarios WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Usuario
            {
                Id = (int)reader["id"],
                Nombre = reader["nombre"].ToString()!,
                Email = reader["email"].ToString()!,
                Rol = reader["rol"].ToString()!,
                Activo = (bool)reader["activo"]
            };
        }

        return null;
    }

    public int ContarAdmins()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT COUNT(*) FROM usuarios WHERE rol = 'admin' AND activo = 1", conn);
        return (int)cmd.ExecuteScalar();
    }

    public void ResetearPassword(int id, string nuevaPassword)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var hash = HashPassword(nuevaPassword);

        var cmd = new SqlCommand("UPDATE usuarios SET password = @Hash WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Hash", hash);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }

    //public void ActualizarPassword(string email, string nuevaPassword)
    //{
    //    using var conn = new SqlConnection(_connectionString);
    //    conn.Open();

    //    var hash = HashPassword(nuevaPassword);

    //    var cmd = new SqlCommand("UPDATE usuarios SET password = @Hash WHERE email = @Email", conn);
    //    cmd.Parameters.AddWithValue("@Hash", hash);
    //    cmd.Parameters.AddWithValue("@Email", email);

    //    cmd.ExecuteNonQuery();
    //}
}