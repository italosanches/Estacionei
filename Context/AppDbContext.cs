using Estacionei.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<HourPriceConfiguration> HourPriceConfigurations { get; set; }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Exit> Exits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // chama as configuracoes padroes do Identity


            modelBuilder.Entity<ApplicationUser>().HasIndex(user => user.Email).IsUnique();
              

            modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.VehicleLicensePlate)
            .IsUnique();

            modelBuilder.Entity<HourPriceConfiguration>()
                .HasIndex(conf => conf.VehicleType)
                .IsUnique();

            modelBuilder.Entity<Entry>()
           .HasOne(e => e.Exit) // Um Entrada tem uma Saida
           .WithOne(s => s.Entry) // Uma Saida tem uma Entrada
           .HasForeignKey<Exit>(s => s.EntryId) // Chave estrangeira da Saida
           .OnDelete(DeleteBehavior.Cascade); // Comportamento ao deletar

            
        }
    }
}
