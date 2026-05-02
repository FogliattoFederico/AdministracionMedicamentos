namespace Geriatrico.Models.Dominio;

using System.ComponentModel.DataAnnotations;

public class MedicamentoPaciente
{
    public int Id { get; set; }
    public int PacienteId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un medicamento")]
    [Display(Name = "Medicamento")]
    public int MedicamentoId { get; set; }

    [Required(ErrorMessage = "La dosis es obligatoria")]
    [StringLength(100, ErrorMessage = "La dosis no puede superar los 100 caracteres")]
    public string Dosis { get; set; } = string.Empty;

    [Required(ErrorMessage = "La frecuencia es obligatoria")]
    [StringLength(100, ErrorMessage = "La frecuencia no puede superar los 100 caracteres")]
    public string Frecuencia { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Horarios { get; set; }

    [StringLength(50)]
    public string? Via { get; set; }

    public string? Indicacion { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de inicio")]
    public DateTime FechaInicio { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de fin")]
    public DateTime? FechaFin { get; set; }

    public bool Activo { get; set; }

    [StringLength(100)]
    [Display(Name = "Prescripto por")]
    public string? PrescriptoPor { get; set; }

    public string? NombreMedicamento { get; set; }
    public string? Presentacion { get; set; }
}