using Core.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public required string Cedula { get; set; }
        public required string Nombres { get; set; }
        public required string Pellidos { get; set; }
        public required string Correo { get; set; }
        public required string Password { get; set; }
       
        public ICollection<TurnoHistorial> TurnoHistoriales { get; set; } = new List<TurnoHistorial>();
        public ICollection<Turnos> Turnos { get; set; } = new List<Turnos>();
    }
}
