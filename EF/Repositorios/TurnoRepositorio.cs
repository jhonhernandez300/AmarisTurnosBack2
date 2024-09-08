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
    public class TurnoRepositorio : BaseRepositorio<Turnos>, ITurnoRepositorio
    {
        private readonly ApplicationDbContext _context;

        public TurnoRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
