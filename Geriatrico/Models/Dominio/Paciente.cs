namespace Geriatrico.Models.Dominio;

using System.ComponentModel.DataAnnotations;

public class Paciente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(20, ErrorMessage = "El DNI no puede superar los 20 caracteres")]
    public string Dni { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de nacimiento")]
    [Range(typeof(DateTime), "01/01/1900", "31/12/2100", ErrorMessage = "La fecha ingresada no es válida")]
    public DateTime FechaNac { get; set; }
    

    [StringLength(10, ErrorMessage = "La habitación no puede superar los 10 caracteres")]
    public string? Habitacion { get; set; }

    [StringLength(100, ErrorMessage = "La obra social no puede superar los 100 caracteres")]
    [Display(Name = "Obra Social")]
    public string? ObraSocial { get; set; }

    [StringLength(50, ErrorMessage = "El nro. de afiliado no puede superar los 50 caracteres")]
    [Display(Name = "Nro. Afiliado")]
    public string? NroAfiliado { get; set; }

    [StringLength(100, ErrorMessage = "El nombre de contacto no puede superar los 100 caracteres")]
    [Display(Name = "Contacto")]
    public string? ContactoNombre { get; set; }

    [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres")]
    [Display(Name = "Teléfono")]
    public string? ContactoTel { get; set; }

    public bool Activo { get; set; }
    public DateTime CreatedAt { get; set; }

    public int Edad => DateTime.Today.Year - FechaNac.Year;
}