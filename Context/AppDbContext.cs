using Estacionei.Models;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        DbSet<Cliente> Clientes {  get; set; }
        DbSet<Veiculo> Veiculos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cliente>()
               .HasMany(v => v.VeiculosCliente)
               .WithOne(c => c.Cliente)
               .HasForeignKey(c => c.ClienteId);
        }
    }
}
