namespace Geriatrico.Controllers;

using Microsoft.AspNetCore.Mvc;
using Geriatrico.Models.Negocio;
using Geriatrico.Models.ViewModels;

public class AccountController : Controller
{
    private readonly UsuarioNegocio _negocio;

    public AccountController(UsuarioNegocio negocio)
    {
        _negocio = negocio;
    }

    // LOGIN GET
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("UsuarioNombre") != null)
            return RedirectToAction("Index", "Home");

        return View();
    }

    // LOGIN POST
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        try
        {
            if (!ModelState.IsValid) return View(model);

            var usuario = _negocio.Login(model.Email, model.Password);

            if (usuario == null)
            {
                ViewBag.Error = "Email o contraseña incorrectos.";
                return View(model);
            }

            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
            HttpContext.Session.SetString("UsuarioRol", usuario.Rol);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al iniciar sesión: " + ex.Message;
            return View(model);
        }
    }

    // LOGOUT
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    // RECUPERAR GET
    public IActionResult Recuperar()
    {
        var ip = HttpContext.Connection.RemoteIpAddress;
        if (ip == null || !System.Net.IPAddress.IsLoopback(ip))
            return NotFound();

        if (HttpContext.Session.GetString("UsuarioNombre") != null)
            return RedirectToAction("Index", "Home");

        return View();
    }

    // RECUPERAR POST
    [HttpPost]
    public IActionResult Recuperar(RecuperarPasswordViewModel model)
    {
        var ip = HttpContext.Connection.RemoteIpAddress;
        if (ip == null || !System.Net.IPAddress.IsLoopback(ip))
            return NotFound();

        try
        {
            if (!ModelState.IsValid) return View(model);

            var usuario = _negocio.ObtenerPorEmail(model.Email);

            if (usuario == null)
            {
                ViewBag.Error = "No existe un usuario con ese email.";
                return View(model);
            }

            _negocio.ResetearPassword(usuario.Id, model.NuevaPassword);
            TempData["Exito"] = "Contraseña actualizada correctamente. Ya podés iniciar sesión.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al recuperar la contraseña: " + ex.Message;
            return View(model);
        }
    }

    //public IActionResult InicializarPassword()
    //{
    //    _negocio.ActualizarPassword("admin@geriatrico.com", "admin123");
    //    return Content("Contraseña actualizada correctamente.");
    //}
}