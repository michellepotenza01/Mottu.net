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
            modelBuilder.Entity<Patio>()
                .HasKey(p => p.NomePatio);

            modelBuilder.Entity<Funcionario>()
                .HasKey(f => f.UsuarioFuncionario);

            modelBuilder.Entity<Moto>()
                .HasKey(m => m.Placa);

            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.UsuarioCliente);

            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Patio)
                .WithMany(p => p.Motos)
                .HasForeignKey(m => m.NomePatio)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Funcionario)
                .WithMany(f => f.Motos)
                .HasForeignKey(m => m.UsuarioFuncionario)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Patio)
                .WithMany(p => p.Funcionarios)
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