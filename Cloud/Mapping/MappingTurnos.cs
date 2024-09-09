using Core.DTOs;
using Core.Modelos;

namespace KontrolarCloud.Mapping
{
    public class MappingTurnos : AutoMapper.Profile
    {
        public MappingTurnos()
        {
            // Mapeo de TurnoCompletoDTO a Turnos
            CreateMap<TurnoCompletoDTO, Turnos>()
                .ForMember(dest => dest.IdTurno, opt => opt.Ignore()); // Ignorar el IdTurno en la creación

            // Mapeo de Turnos a TurnoCompletoDTO
            CreateMap<Turnos, TurnoCompletoDTO>();

            // Mapeo de TurnosDTO a Turnos
            CreateMap<TurnosDTO, Turnos>()
                .ForMember(dest => dest.IdTurno, opt => opt.Ignore()); // Ignorar el IdTurno en la creación
        }
    }
}
