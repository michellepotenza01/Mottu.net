MottuDbContext:
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MottuApi.Data
{
    public class MottuDbContext : DbContext
    {
        public MottuDbContext(DbContextOptions<MottuDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Patio)
                .WithMany(p => p.Motos)
                .HasForeignKey(m => m.NomePatio)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Funcionario)
                .WithMany()
                .HasForeignKey(m => m.UsuarioFuncionario)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Patio)
                .WithMany()
                .HasForeignKey(f => f.NomePatio)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Moto)
                .WithMany()
                .HasForeignKey(c => c.MotoPlaca)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}
