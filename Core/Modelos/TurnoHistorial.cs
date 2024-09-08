using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modelos
{
    public class TurnoHistorial
    {
        public int IdTurnoHistorial { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdTurno { get; set; }
        public Turnos Turno { get; set; }
        public string EstadoAnterior { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}
