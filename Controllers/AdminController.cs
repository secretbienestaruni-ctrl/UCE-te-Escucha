using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plataforma_Virtual.Data;
using Plataforma_Virtual.Models;

namespace Plataforma_Virtual.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool EstaAutenticado()
        {
            return HttpContext.Session.GetString("Admin") == "SI";
        }

        public async Task<IActionResult> Index(string buscar)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Cuenta");

            var mensajes = _context.Mensajes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                mensajes = mensajes.Where(m =>
                    (m.Codigo != null && m.Codigo.Contains(buscar)) ||
                    (m.Nombre != null && m.Nombre.Contains(buscar)) ||
                    m.Contenido.Contains(buscar));
            }

            var lista = await mensajes
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();

            var vm = new AdminDashboardViewModel
            {
                Mensajes = lista,
                Total = lista.Count,
                Pendientes = lista.Count(x => x.Estado == "Pendiente"),
                EnRevision = lista.Count(x => x.Estado == "En revisión"),
                Atendidos = lista.Count(x => x.Estado == "Atendido")
            };

            return View(vm);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Cuenta");

            var mensaje = await _context.Mensajes.FindAsync(id);

            if (mensaje == null)
                return NotFound();

            return View(mensaje);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Cuenta");

            var mensaje = await _context.Mensajes.FindAsync(id);

            if (mensaje == null)
                return NotFound();

            mensaje.Estado = estado;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Detalle), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (!EstaAutenticado())
                return RedirectToAction("Login", "Cuenta");

            var mensaje = await _context.Mensajes.FindAsync(id);

            if (mensaje == null)
                return NotFound();

            _context.Mensajes.Remove(mensaje);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}