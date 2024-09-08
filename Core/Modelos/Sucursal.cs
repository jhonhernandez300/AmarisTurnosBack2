using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modelos
{
    public class Sucursal
    {
        public int IdSucursal { get; set; }
        public string NombreSucursal { get; set; }
        public string Direccion { get; set; }

        public ICollection<Turnos> Turnos { get; set; } = new List<Turnos>();
    }
}
