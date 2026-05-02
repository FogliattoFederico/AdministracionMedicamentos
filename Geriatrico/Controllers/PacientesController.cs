namespace Geriatrico.Controllers;

using Microsoft.AspNetCore.Mvc;
using Geriatrico.Models.Negocio;
using Geriatrico.Models.Dominio;
using Geriatrico.Models.ViewModels;

public class PacientesController : Controller
{
    private readonly PacienteNegocio _negocio;
    private readonly MedicamentoPacienteNegocio _medicamentoNegocio;
    private bool EstaLogueado() =>
        HttpContext.Session.GetString("UsuarioNombre") != null;
    
    public PacientesController(PacienteNegocio negocio, MedicamentoPacienteNegocio medicamentoNegocio)
    {
        _negocio = negocio;
        _medicamentoNegocio = medicamentoNegocio;
    }

    // LISTAR
    public IActionResult Index(string? buscar, int pagina = 1)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");

        try
        {
            const int porPagina = 10;
            var total = _negocio.ContarPacientes(buscar);

            var viewModel = new PacienteListaViewModel
            {
                Pacientes = _negocio.ListarPaginado(pagina, porPagina, buscar),
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)total / porPagina),
                Buscar = buscar,
                PorPagina = porPagina
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al cargar los pacientes: " + ex.Message;
            return View(new PacienteListaViewModel());
        }
    }

    // DETALLE
    public IActionResult Detalle(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");

        try
        {
            var paciente = _negocio.ObtenerPorId(id);
            if (paciente == null) return NotFound();

            var viewModel = new PacienteDetalleViewModel
            {
                Paciente = paciente,
                Medicamentos = _medicamentoNegocio.ListarPorPaciente(id)
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al cargar el detalle: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    // FORMULARIO AGREGAR
    public IActionResult Agregar()
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        return View();
    }

    [HttpPost]
    public IActionResult Agregar(Paciente p)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            if (!ModelState.IsValid) return View(p);
            _negocio.Agregar(p);
            TempData["Exito"] = "Paciente agregado correctamente.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al agregar el paciente: " + ex.Message;
            return View(p);
        }
    }

    // FORMULARIO MODIFICAR
    public IActionResult Modificar(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            var paciente = _negocio.ObtenerPorId(id);
            if (paciente == null) return NotFound();
            return View(paciente);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al cargar el paciente: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult Modificar(Paciente p)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            if (!ModelState.IsValid) return View(p);
            _negocio.Modificar(p);
            TempData["Exito"] = "Paciente modificado correctamente.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al modificar el paciente: " + ex.Message;
            return View(p);
        }
    }

    // ELIMINAR
    public IActionResult Eliminar(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            _negocio.Eliminar(id);
            TempData["Exito"] = "Paciente dado de baja correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al eliminar el paciente: " + ex.Message;
        }

        return RedirectToAction("Index");
    }

    public IActionResult Imprimir(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");

        try
        {
            var paciente = _negocio.ObtenerPorId(id);
            if (paciente == null) return NotFound();

            var viewModel = new PacienteDetalleViewModel
            {
                Paciente = paciente,
                Medicamentos = _medicamentoNegocio.ListarPorPaciente(id)
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al generar la impresión: " + ex.Message;
            return RedirectToAction("Detalle", new { id });
        }
    }
}