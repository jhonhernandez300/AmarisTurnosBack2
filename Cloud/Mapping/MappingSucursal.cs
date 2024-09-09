using AutoMapper;
using Core.Models;
using Core.DTOs;
using Core;
using Core.Modelos;

namespace KontrolarCloud.Mapping
{
    public class MappingSucursal : AutoMapper.Profile
    { 
        public MappingSucursal()
        {
            CreateMap<SucursalDTO, Sucursal>();
        }    
    }
}
