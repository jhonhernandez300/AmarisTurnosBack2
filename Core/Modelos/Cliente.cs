using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modelos
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }        
        public required string Nombres { get; set; }
        public required string Pellidos { get; set; }
    }
}
