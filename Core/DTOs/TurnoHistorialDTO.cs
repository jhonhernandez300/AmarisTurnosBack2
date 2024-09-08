using Core.Modelos;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    [Table("Turno_Historial")]
    public class TurnoHistorialDTO
    {
        [Key]        
        public int IdTurnoHistorial { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } 

        [ForeignKey("Turnos")]
        public int IdTurno { get; set; }
        public Turnos Turno { get; set; } 

        [StringLength(20)]
        public string EstadoAnterior { get; set; }

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}
