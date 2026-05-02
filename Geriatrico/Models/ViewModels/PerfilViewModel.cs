namespace Geriatrico.Models.ViewModels;

using System.ComponentModel.DataAnnotations;

public class PerfilViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Nueva contraseña")]
    public string? NuevaPassword { get; set; }

    [Compare("NuevaPassword", ErrorMessage = "Las contraseñas no coinciden")]
    [Display(Name = "Confirmar contraseña")]
    public string? ConfirmarPassword { get; set; }
}