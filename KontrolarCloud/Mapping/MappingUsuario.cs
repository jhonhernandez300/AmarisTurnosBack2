using AutoMapper;
using Core.Models;
using Core.DTOs;
using Core;

namespace KontrolarCloud.Mapping
{
    public class MappingUsuario : AutoMapper.Profile
    {
        public MappingUsuario()
        {
            CreateMap<UsuarioDTO, Core.Models.Usuario>();                       
        }
    }
}
