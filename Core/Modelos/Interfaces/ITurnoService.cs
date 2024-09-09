using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modelos.Interfaces
{
    public interface ITurnoService
    {
        Task<int> CreateTurnoAsync(Turnos turnos);
        Task<(bool success, string message)> UpdateTurnoAsync(Turnos turnos);
        Task<(Turnos turno, string message)> GetTurnoByIdAsync(int id);
        Task<List<Turnos>> GetTurnosActivadosAsync();
        Task<int> ContarTurnosPorUsuarioAsync(string idUsuario);
    }
}
