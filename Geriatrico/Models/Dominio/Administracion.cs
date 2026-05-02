// Models/Dominio/Administracion.cs
namespace Geriatrico.Models.Dominio;

public class Administracion
{
    public int Id { get; set; }
    public int MedicamentoPacId { get; set; }
    public DateTime Fecha { get; set; }
    public string HoraProgramada { get; set; } = string.Empty;
    public string? HoraAdministrada { get; set; }
    public string? AdministradoPor { get; set; }
    public bool Tomado { get; set; }
    public string? Observaciones { get; set; }

    // Para mostrar en la vista
    public string? NombreMedicamento { get; set; }
    public string? NombrePaciente { get; set; }
    public string? Dosis { get; set; }
}