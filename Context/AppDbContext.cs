﻿using Estacionei.Models;
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
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }

        public DbSet<ConfiguracaoValorHora> ConfiguracoesValoresHora { get; set; }

        public DbSet<Entrada> Entrada { get; set; }
        public DbSet<Saida> Saida { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // chama as configuracoes padroes do Identity


            modelBuilder.Entity<ApplicationUser>().HasIndex(user => user.Email).IsUnique();
              

            modelBuilder.Entity<Veiculo>()
            .HasIndex(v => v.VeiculoPlaca)
            .IsUnique();

            modelBuilder.Entity<ConfiguracaoValorHora>()
                .HasIndex(conf => conf.TipoVeiculo)
                .IsUnique();

            modelBuilder.Entity<Entrada>()
           .HasOne(e => e.Saida) // Um Entrada tem uma Saida
           .WithOne(s => s.Entrada) // Uma Saida tem uma Entrada
           .HasForeignKey<Saida>(s => s.EntradaId) // Chave estrangeira da Saida
           .OnDelete(DeleteBehavior.Cascade); // Comportamento ao deletar

            
        }
    }
}
