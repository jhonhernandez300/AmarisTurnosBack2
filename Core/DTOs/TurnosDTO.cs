using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class TurnosDTO
    {        
        public int IdUsuario { get; set; }
        public int IdSucursal { get; set; }
        public DateTime FechaTurno { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
