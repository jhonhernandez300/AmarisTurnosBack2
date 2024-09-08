using Core.Interfaces;
using Core.Modelos.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {               
        IUsuarioRepositorio Usuarios { get; }
        ISucursalRepositorio Sucursales { get; }
        ITurnoRepositorio Turnos { get; }
        ITurnoHistorialRepositorio TurnoHistoriales { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
