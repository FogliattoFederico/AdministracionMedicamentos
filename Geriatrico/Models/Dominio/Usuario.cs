namespace Geriatrico.Models.Dominio;

using System.ComponentModel.DataAnnotations;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = string.Empty;

    public bool Activo { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Rol { get; set; } = "empleado";
}