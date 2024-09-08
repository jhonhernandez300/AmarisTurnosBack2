using Core.Modelos;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Turnos> Turnos { get; set; }
        public DbSet<TurnoHistorial> TurnoHistoriales { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasKey(df => df.IdCliente);

            modelBuilder.Entity<Usuario>()
                .HasKey(df => df.IdUsuario);

            modelBuilder.Entity<Sucursal>()
                .HasKey(df => df.IdSucursal);

            modelBuilder.Entity<Turnos>()
                .HasKey(df => df.IdTurno);

            modelBuilder.Entity<TurnoHistorial>()
                .HasKey(df => df.IdTurnoHistorial);

            //modelBuilder.Entity<Turnos>()
            //    .HasOne(df => df.Usuario)
            //    .WithMany(f => f.Turnos)
            //    .HasForeignKey(df => df.IdUsuario);

            //modelBuilder.Entity<TurnoHistorial>()
            //    .HasOne(df => df.Usuario)
            //    .WithMany(f => f.TurnoHistoriales)
            //    .HasForeignKey(df => df.IdUsuario);

            //modelBuilder.Entity<Turnos>()
            //    .HasOne(df => df.Sucursal)
            //    .WithMany(f => f.Turnos)
            //    .HasForeignKey(df => df.IdTurno);            

            base.OnModelCreating(modelBuilder);
        }
    }

}
