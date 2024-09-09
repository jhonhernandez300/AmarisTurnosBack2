using Core.DTOs;
using Core.Modelos;

namespace KontrolarCloud.Mapping
{
    public class MappingTurnos : AutoMapper.Profile
    {
        public MappingTurnos()
        {            
            CreateMap<TurnoCompletoDTO, Turnos>();   
            
            CreateMap<Turnos, TurnoCompletoDTO>();
            
            CreateMap<TurnosDTO, Turnos>()
                .ForMember(dest => dest.IdTurno, opt => opt.Ignore()); 
        }
    }
}
