using Microsoft.EntityFrameworkCore;
using Plataforma_Virtual.Models;

namespace Plataforma_Virtual.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mensaje> Mensajes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mensaje>()
                .Property(m => m.Codigo)
                .HasMaxLength(20);

            modelBuilder.Entity<Mensaje>()
                .Property(m => m.Estado)
                .HasMaxLength(30);

            modelBuilder.Entity<Mensaje>()
                .Property(m => m.Nombre)
                .HasMaxLength(100);

            modelBuilder.Entity<Mensaje>()
                .Property(m => m.Correo)
                .HasMaxLength(150);
        }
    }
}