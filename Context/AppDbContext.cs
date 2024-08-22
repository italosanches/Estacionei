using Estacionei.Models;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Veiculo>()
            .HasIndex(v => v.VeiculoPlaca)
            .IsUnique();
        }
    }
}
