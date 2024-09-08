using Core.Interfaces;
using Core.Modelos;
using Core.Modelos.Interfaces;
using Core.Models;
using EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Repositorios
{
    public class SucursalRepositorio : BaseRepositorio<Sucursal>, ISucursalRepositorio
    {
        private readonly ApplicationDbContext _context;

        public SucursalRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
