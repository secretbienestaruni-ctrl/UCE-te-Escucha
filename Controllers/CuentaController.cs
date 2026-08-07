using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Plataforma_Virtual.Controllers;

public class CuentaController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string usuario, string password)
    {
        if (usuario == "UCE2026" && password == "Secret1468")
        {
            HttpContext.Session.SetString("Admin", "SI");
            return RedirectToAction("Index", "Admin");
        }

        ViewBag.Error = "Usuario o contraseña incorrectos.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }
}