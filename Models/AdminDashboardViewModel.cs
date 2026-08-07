using Plataforma_Virtual.Models;

namespace Plataforma_Virtual.Models
{
    public class AdminDashboardViewModel
    {
        public List<Mensaje> Mensajes { get; set; } = new();

        public int Total { get; set; }

        public int Pendientes { get; set; }

        public int EnRevision { get; set; }

        public int Atendidos { get; set; }
    }
}