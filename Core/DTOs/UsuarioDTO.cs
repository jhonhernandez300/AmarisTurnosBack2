using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class UsuarioDTO
    {        
        public int IdUsuario { get; set; }
        public required string Cedula { get; set; }
        public required string Nombres { get; set; }
        public required string Apellidos { get; set; }
        public required string Correo { get; set; }
        public required string Password { get; set; }
    }
}
