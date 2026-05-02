namespace Geriatrico.Models.ViewModels;

using Geriatrico.Models.Dominio;

public class PacienteListaViewModel
{
    public List<Paciente> Pacientes { get; set; } = new List<Paciente>();
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public string? Buscar { get; set; }
    public int PorPagina { get; set; } = 10;

    public bool TienePaginaAnterior => PaginaActual > 1;
    public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
}