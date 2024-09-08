using Core.DTOs;
using Core.Modelos;

namespace KontrolarCloud.Mapping
{
    public class MappingTurnos : AutoMapper.Profile
    {
        public MappingTurnos()
        {
            CreateMap<TurnosDTO, Turnos>();
        }
    }
}
