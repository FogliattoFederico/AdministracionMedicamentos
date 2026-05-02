// Models/ViewModels/HojaDiariaViewModel.cs
namespace Geriatrico.Models.ViewModels;

using Geriatrico.Models.Dominio;

public class HojaDiariaViewModel
{
    public Paciente Paciente { get; set; } = new Paciente();
    public DateTime Fecha { get; set; } = DateTime.Today;
    public List<Administracion> Administraciones { get; set; } = new List<Administracion>();
}