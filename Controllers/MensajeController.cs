using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plataforma_Virtual.Data;
using Plataforma_Virtual.Models;
using Plataforma_Virtual.Services;

namespace Plataforma_Virtual.Controllers;

public class MensajeController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly EmailService _emailService;

    public MensajeController(
        AppDbContext context,
        IWebHostEnvironment environment,
        EmailService emailService)
    {
        _context = context;
        _environment = environment;
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Mensaje());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Mensaje mensaje)
    {
        if (!ModelState.IsValid)
            return View(mensaje);

        // Si el usuario envía el mensaje de forma anónima
        if (mensaje.Anonimo)
        {
            mensaje.Nombre = null;
            mensaje.Correo = null;
        }

        // Guardar archivo adjunto
        if (mensaje.Archivo != null && mensaje.Archivo.Length > 0)
        {
            string carpeta = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string nombreArchivo = Guid.NewGuid().ToString() +
                                   Path.GetExtension(mensaje.Archivo.FileName);

            string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await mensaje.Archivo.CopyToAsync(stream);
            }

            mensaje.RutaArchivo = "/uploads/" + nombreArchivo;
        }

        // Generar código
        int ultimoId = 0;

        if (await _context.Mensajes.AnyAsync())
        {
            ultimoId = await _context.Mensajes.MaxAsync(x => x.Id);
        }

        mensaje.Codigo = $"UCE-{(ultimoId + 1):D6}";
        mensaje.Estado = "Pendiente";
        mensaje.Fecha = DateTime.Now;

        _context.Mensajes.Add(mensaje);

        await _context.SaveChangesAsync();

        // Enviar correo
        try
        {
            string asunto = $"Nuevo mensaje recibido - {mensaje.Codigo}";

            string cuerpo = $@"
<h2>Nuevo mensaje recibido</h2>

<p><strong>Código:</strong> {mensaje.Codigo}</p>

<p><strong>Nombre:</strong> {(mensaje.Anonimo ? "ANÓNIMO" : mensaje.Nombre)}</p>

<p><strong>Correo:</strong> {(string.IsNullOrWhiteSpace(mensaje.Correo) ? "-" : mensaje.Correo)}</p>

<p><strong>Fecha:</strong> {mensaje.Fecha:dd/MM/yyyy HH:mm}</p>

<p><strong>Estado:</strong> {mensaje.Estado}</p>

<hr>

<p><strong>Mensaje:</strong></p>

<p>{mensaje.Contenido}</p>";

            await _emailService.EnviarCorreoAsync(asunto, cuerpo);
        }
        catch
        {
            // Si falla el correo, el mensaje ya quedó guardado.
        }

        return RedirectToAction(nameof(Confirmacion), new
        {
            codigo = mensaje.Codigo
        });
    }

    [HttpGet]
    public IActionResult Confirmacion(string codigo)
    {
        ViewBag.Codigo = codigo;
        return View();
    }
}