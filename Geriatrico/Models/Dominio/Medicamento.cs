namespace Geriatrico.Models.Dominio;

using System.ComponentModel.DataAnnotations;

public class Medicamento
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(150, ErrorMessage = "El nombre genérico no puede superar los 150 caracteres")]
    [Display(Name = "Nombre Genérico")]
    public string? NombreGenerico { get; set; }

    [StringLength(100, ErrorMessage = "La presentación no puede superar los 100 caracteres")]
    [Display(Name = "Presentación")]
    public string? Presentacion { get; set; }

    [StringLength(100, ErrorMessage = "El laboratorio no puede superar los 100 caracteres")]
    public string? Laboratorio { get; set; }
}