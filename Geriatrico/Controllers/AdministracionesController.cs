namespace Geriatrico.Controllers;

using Microsoft.AspNetCore.Mvc;
using Geriatrico.Models.Negocio;
using Geriatrico.Models.Dominio;
using Geriatrico.Models.ViewModels;

public class AdministracionesController : Controller
{
    private readonly AdministracionNegocio _negocio;
    private readonly PacienteNegocio _pacienteNegocio;
    private bool EstaLogueado() =>
    HttpContext.Session.GetString("UsuarioNombre") != null;

    public AdministracionesController(AdministracionNegocio negocio, PacienteNegocio pacienteNegocio)
    {
        _negocio = negocio;
        _pacienteNegocio = pacienteNegocio;
    }

    // HOJA DIARIA
    public IActionResult HojaDiaria(int pacienteId, DateTime? fecha)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            var fechaConsulta = fecha ?? DateTime.Today;
            var paciente = _pacienteNegocio.ObtenerPorId(pacienteId);
            if (paciente == null) return NotFound();

            // Generar administraciones del dia si no existen
            _negocio.GenerarDelDia(pacienteId, fechaConsulta);

            var viewModel = new HojaDiariaViewModel
            {
                Paciente = paciente,
                Fecha = fechaConsulta,
                Administraciones = _negocio.ListarPorPacienteYFecha(pacienteId, fechaConsulta)
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al cargar la hoja diaria: " + ex.Message;
            return RedirectToAction("Detalle", "Pacientes", new { id = pacienteId });
        }
    }

    // REGISTRAR ADMINISTRACION
    [HttpPost]
    public IActionResult Registrar(Administracion a, int pacienteId, DateTime fecha)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            _negocio.Registrar(a);
            TempData["Exito"] = "Administración registrada correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al registrar: " + ex.Message;
        }

        return RedirectToAction("HojaDiaria", new { pacienteId, fecha });
    }
}