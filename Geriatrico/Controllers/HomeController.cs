namespace Geriatrico.Controllers;

using Microsoft.AspNetCore.Mvc;
using Geriatrico.Models.Negocio;
using Geriatrico.Models.ViewModels;

public class HomeController : Controller
{
    private readonly DashboardNegocio _dashboardNegocio;
    private readonly PacienteNegocio _pacienteNegocio;
    private bool EstaLogueado() =>
        HttpContext.Session.GetString("UsuarioNombre") != null;

    public HomeController(DashboardNegocio dashboardNegocio, PacienteNegocio pacienteNegocio)
    {
        _dashboardNegocio = dashboardNegocio;
        _pacienteNegocio = pacienteNegocio;
    }

    public IActionResult Index()
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            var viewModel = new DashboardViewModel
            {
                TotalPacientes = _dashboardNegocio.TotalPacientes(),
                TotalMedicamentos = _dashboardNegocio.TotalMedicamentos(),
                AdministracionesTomadas = _dashboardNegocio.AdministracionesHoyTomadas(),
                AdministracionesPendientes = _dashboardNegocio.AdministracionesHoyPendientes(),
                UltimosPacientes = _pacienteNegocio.UltimosAgregados()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al cargar el dashboard: " + ex.Message;
            return View(new DashboardViewModel());
        }
    }
}