using Core;
using Core.Interfaces;
using Core.Modelos;
using Core.Modelos.Interfaces;
using Core.Models;
using EF.Repositories;
using EF.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;        

        public UnitOfWork(
            ApplicationDbContext context,                            
            IBaseRepositorio<Usuario> usuarios,
            ISucursalRepositorio sucursales,
            ITurnoRepositorio turnos,
            ITurnoHistorialRepositorio turnoHistoriales
        )                          
        {
            _context = context;
            Usuarios = new UsuarioRepositorio(_context);
            Sucursales = sucursales;
            Turnos = turnos;
            TurnoHistoriales = turnoHistoriales;
        }

        public IUsuarioRepositorio Usuarios { get; private set; }
        public ISucursalRepositorio Sucursales { get; private set; }
        public ITurnoRepositorio Turnos { get; private set; }
        public ITurnoHistorialRepositorio TurnoHistoriales { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            var primaryResult = await _context.SaveChangesAsync();            
            return primaryResult;
        }

        public void Dispose()
        {
            _context.Dispose();            
        }
    }

}
