using Core.DTOs;
using Core.Modelos;

namespace KontrolarCloud.Mapping
{
    public class MappingTurnoHistorial : AutoMapper.Profile
    {
        public MappingTurnoHistorial()
        {
            CreateMap<TurnoHistorialDTO, TurnoHistorial>();
        }
    }
}
