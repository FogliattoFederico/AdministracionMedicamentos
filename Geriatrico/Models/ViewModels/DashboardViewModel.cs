namespace Geriatrico.Models.ViewModels;

using Geriatrico.Models.Dominio;

public class DashboardViewModel
{
    public int TotalPacientes { get; set; }
    public int TotalMedicamentos { get; set; }
    public int AdministracionesTomadas { get; set; }
    public int AdministracionesPendientes { get; set; }
    public List<Paciente> UltimosPacientes { get; set; } = new List<Paciente>();
}