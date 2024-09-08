using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modelos
{
    [Table("Turnos")]
    public class Turnos
    {
        [Key]        
        public int IdTurno { get; set; }
        public int IdUsuario { get; set; }
        public int IdSucursal { get; set; }
        public DateTime FechaTurno { get; set; }
        public string Estado { get; set; }   
    }
}
