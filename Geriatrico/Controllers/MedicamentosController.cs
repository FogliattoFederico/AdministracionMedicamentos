namespace Geriatrico.Controllers;

using Microsoft.AspNetCore.Mvc;
using Geriatrico.Models.Negocio;
using Geriatrico.Models.Dominio;

public class MedicamentosController : Controller
{
    private readonly MedicamentoNegocio _negocio;
    private bool EstaLogueado() =>
    HttpContext.Session.GetString("UsuarioNombre") != null;

    public MedicamentosController(MedicamentoNegocio negocio)
    {
        _negocio = negocio;
    }

    // LISTAR
    public IActionResult Index()
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");

        try
        {
            var medicamentos = _negocio.Listar();
            return View(medicamentos);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al cargar los medicamentos: " + ex.Message;
            return View(new List<Medicamento>());
        }
    }

    // DETALLE
    public IActionResult Detalle(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            var medicamento = _negocio.ObtenerPorId(id);
            if (medicamento == null) return NotFound();
            return View(medicamento);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al cargar el medicamento: " + ex.Message;
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
    public IActionResult Agregar(Medicamento m)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            if (!ModelState.IsValid) return View(m);
            _negocio.Agregar(m);
            TempData["Exito"] = "Medicamento agregado correctamente.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al agregar el medicamento: " + ex.Message;
            return View(m);
        }
    }

    // FORMULARIO MODIFICAR
    public IActionResult Modificar(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            var medicamento = _negocio.ObtenerPorId(id);
            if (medicamento == null) return NotFound();
            return View(medicamento);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al cargar el medicamento: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult Modificar(Medicamento m)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            if (!ModelState.IsValid) return View(m);
            _negocio.Modificar(m);
            TempData["Exito"] = "Medicamento modificado correctamente.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al modificar el medicamento: " + ex.Message;
            return View(m);
        }
    }

    // ELIMINAR
    public IActionResult Eliminar(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        try
        {
            _negocio.Eliminar(id);
            TempData["Exito"] = "Medicamento eliminado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al eliminar el medicamento: " + ex.Message;
        }

        return RedirectToAction("Index");
    }
}