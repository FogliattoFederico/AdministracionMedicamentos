namespace Geriatrico.Controllers;

using Geriatrico.Models.Dominio;
using Geriatrico.Models.Negocio;
using Geriatrico.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class UsuariosController : Controller
{
    private readonly UsuarioNegocio _negocio;

    public UsuariosController(UsuarioNegocio negocio)
    {
        _negocio = negocio;
    }

    // LISTAR
    public IActionResult Index()
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        if (!EsAdmin()) return RedirectToAction("Index", "Home");

        try
        {
            var usuarios = _negocio.Listar();
            return View(usuarios);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al cargar los usuarios: " + ex.Message;
            return View(new List<Usuario>());
        }
    }

    // AGREGAR GET
    public IActionResult Agregar()
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        if (!EsAdmin()) return RedirectToAction("Index", "Home");
        return View();
    }

    // AGREGAR POST
    [HttpPost]
    public IActionResult Agregar(Usuario u)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        if (!EsAdmin()) return RedirectToAction("Index", "Home");

        try
        {
            if (!ModelState.IsValid) return View(u);
            u.Password = UsuarioNegocio.HashPassword(u.Password);
            _negocio.Agregar(u);
            TempData["Exito"] = "Usuario agregado correctamente.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al agregar el usuario: " + ex.Message;
            return View(u);
        }
    }

    // ELIMINAR
    public IActionResult Eliminar(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        if (!EsAdmin()) return RedirectToAction("Index", "Home");

        try
        {
            // Verificar que no sea el último admin
            var usuario = _negocio.ObtenerPorId(id);
            if (usuario?.Rol == "admin" && _negocio.ContarAdmins() <= 1)
            {
                TempData["Error"] = "No podés eliminar el único administrador del sistema.";
                return RedirectToAction("Index");
            }

            _negocio.Eliminar(id);
            TempData["Exito"] = "Usuario dado de baja correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al eliminar el usuario: " + ex.Message;
        }

        return RedirectToAction("Index");
    }

    private bool EstaLogueado() =>
        HttpContext.Session.GetString("UsuarioNombre") != null;

    private bool EsAdmin() =>
        HttpContext.Session.GetString("UsuarioRol") == "admin";

    // PERFIL GET
    public IActionResult Perfil()
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");

        try
        {
            var email = HttpContext.Session.GetString("UsuarioEmail")!;
            var usuario = _negocio.ObtenerPorEmail(email);
            if (usuario == null) return NotFound();

            var viewModel = new PerfilViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al cargar el perfil: " + ex.Message;
            return RedirectToAction("Index", "Home");
        }
    }

    // PERFIL POST
    [HttpPost]
    public IActionResult Perfil(PerfilViewModel model)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");

        try
        {
            if (!ModelState.IsValid) return View(model);

            var email = HttpContext.Session.GetString("UsuarioEmail")!;
            var usuario = _negocio.ObtenerPorEmail(email);
            if (usuario == null) return NotFound();

            usuario.Nombre = model.Nombre;
            usuario.Email = model.Email;

            // Solo cambia la contraseña si ingresó una nueva
            if (!string.IsNullOrWhiteSpace(model.NuevaPassword))
                usuario.Password = UsuarioNegocio.HashPassword(model.NuevaPassword);

            _negocio.ModificarPerfil(usuario);

            // Actualizar sesión con los nuevos datos
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email);

            TempData["Exito"] = "Perfil actualizado correctamente.";
            return RedirectToAction("Perfil");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al actualizar el perfil: " + ex.Message;
            return View(model);
        }
    }

    // RESETEAR PASSWORD GET
    public IActionResult ResetearPassword(int id)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        if (!EsAdmin()) return RedirectToAction("Index", "Home");

        try
        {
            var usuario = _negocio.ObtenerPorId(id);
            if (usuario == null) return NotFound();

            var viewModel = new ResetPasswordViewModel
            {
                UsuarioId = usuario.Id,
                NombreUsuario = usuario.Nombre
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al cargar el formulario: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    // RESETEAR PASSWORD POST
    [HttpPost]
    public IActionResult ResetearPassword(ResetPasswordViewModel model)
    {
        if (!EstaLogueado()) return RedirectToAction("Login", "Account");
        if (!EsAdmin()) return RedirectToAction("Index", "Home");

        try
        {
            if (!ModelState.IsValid) return View(model);

            _negocio.ResetearPassword(model.UsuarioId, model.NuevaPassword);
            TempData["Exito"] = $"Contraseña de {model.NombreUsuario} reseteada correctamente.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al resetear la contraseña: " + ex.Message;
            return View(model);
        }
    }
}