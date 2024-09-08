using Core.Modelos.Interfaces;
using Core.Modelos;
using EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Repositorios
{
    public class TurnoHistorialRepositorio : BaseRepositorio<TurnoHistorial>, ITurnoHistorialRepositorio
    {
        private readonly ApplicationDbContext _context;

        public TurnoHistorialRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
