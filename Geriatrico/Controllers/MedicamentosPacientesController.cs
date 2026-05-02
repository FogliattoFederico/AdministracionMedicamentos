namespace Geriatrico.Controllers;

using Microsoft.AspNetCore.Mvc;
using Geriatrico.Models.Negocio;
using Geriatrico.Models.Dominio;

public class MedicamentosPacientesController : Controller
{
    private readonly MedicamentoPacienteNegocio _negocio;
    private readonly MedicamentoNegocio _medicamentoNegocio;
    private bool EstaLogueado() =>
    HttpContext.Session.GetString("UsuarioNombre") != null;

    public MedicamentosPacientesController(MedicamentoPacienteNegocio negocio, MedicamentoNegocio medicamentoNegocio)
    {
        _negocio = negocio;
        _medicamentoNegocio = medicamentoNegocio;
    }

    // FORMULARIO AGREGAR
    public IActionResult Agregar(int pacienteId)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");

        try
        {
            ViewBag.Medicamentos = _medicamentoNegocio.Listar();
            return View(new MedicamentoPaciente { PacienteId = pacienteId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al cargar el formulario: " + ex.Message;
            return RedirectToAction("Detalle", "Pacientes", new { id = pacienteId });
        }
    }

    [HttpPost]
    public IActionResult Agregar(MedicamentoPaciente mp)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Medicamentos = _medicamentoNegocio.Listar();
                return View(mp);
            }
            _negocio.Agregar(mp);
            TempData["Exito"] = "Medicamento asignado correctamente.";
            return RedirectToAction("Detalle", "Pacientes", new { id = mp.PacienteId });
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al asignar el medicamento: " + ex.Message;
            ViewBag.Medicamentos = _medicamentoNegocio.Listar();
            return View(mp);
        }
    }

    // FORMULARIO MODIFICAR
    public IActionResult Modificar(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            var mp = _negocio.ObtenerPorId(id);
            if (mp == null) return NotFound();
            ViewBag.Medicamentos = _medicamentoNegocio.Listar();
            return View(mp);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al cargar el formulario: " + ex.Message;
            return RedirectToAction("Index", "Pacientes");
        }
    }

    [HttpPost]
    public IActionResult Modificar(MedicamentoPaciente mp)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Medicamentos = _medicamentoNegocio.Listar();
                return View(mp);
            }
            _negocio.Modificar(mp);
            TempData["Exito"] = "Medicamento modificado correctamente.";
            return RedirectToAction("Detalle", "Pacientes", new { id = mp.PacienteId });
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al modificar el medicamento: " + ex.Message;
            ViewBag.Medicamentos = _medicamentoNegocio.Listar();
            return View(mp);
        }
    }

    // ELIMINAR
    public IActionResult Eliminar(int id, int pacienteId)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            _negocio.Eliminar(id);
            TempData["Exito"] = "Medicamento quitado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al quitar el medicamento: " + ex.Message;
        }

        return RedirectToAction("Detalle", "Pacientes", new { id = pacienteId });
    }
}