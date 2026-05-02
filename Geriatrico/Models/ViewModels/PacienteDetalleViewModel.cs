namespace Geriatrico.Models.ViewModels;

using Geriatrico.Models.Dominio;

public class PacienteDetalleViewModel
{
    public Paciente Paciente { get; set; } = new Paciente();
    public List<MedicamentoPaciente> Medicamentos { get; set; } = new List<MedicamentoPaciente>();
}