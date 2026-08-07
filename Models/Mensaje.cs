using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Plataforma_Virtual.Models
{
    public class Mensaje
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Código")]
        public string Codigo { get; set; } = "";

        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress]
        public string? Correo { get; set; }

        public bool Anonimo { get; set; }

        [Required(ErrorMessage = "Debe escribir un mensaje.")]
        [Display(Name = "Mensaje")]
        public string Contenido { get; set; } = "";

        // Ruta del archivo guardado
        public string? RutaArchivo { get; set; }

        // Archivo que sube el usuario (NO se guarda en SQL)
        [NotMapped]
        public IFormFile? Archivo { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        // Pendiente - En revisión - Atendido
        public string Estado { get; set; } = "Pendiente";
    }
}