using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class SucursalDTO
    {
        public int IdSucursal { get; set; }
        public required string NombreSucursal { get; set; }
        public required string Direccion { get; set; }
    }
}
