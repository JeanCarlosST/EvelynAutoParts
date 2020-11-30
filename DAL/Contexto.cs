using Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class Contexto : DbContext
    {
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Cobros> Cobros { get; set; }
        public DbSet<CobrosDetalle> CobrosDetalle { get; set; }
        public DbSet<Facturas> Facturas { get; set; }
        public DbSet<FacturasDetalle> FacturasDetalle { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Vendedores> Vendedores { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite(@"Data Source=..\Data\EvelynAutoParts.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuarios>().HasData(new Usuarios(1, "jean", "admin", "Jean Carlos", "Santos"));
            modelBuilder.Entity<Usuarios>().HasData(new Usuarios(2, "david", "admin", "David", "Maria"));
        } 
    }
}
