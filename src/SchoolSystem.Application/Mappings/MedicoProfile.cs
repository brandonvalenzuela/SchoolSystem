using AutoMapper;
using SchoolSystem.Application.DTOs.Medico;
using SchoolSystem.Domain.Entities.Medico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class MedicoProfile : Profile
    {
        public MedicoProfile()
        {
            CreateMap<ExpedienteMedico, ExpedienteMedicoDto>()
                .ForMember(d => d.NombreAlumno, o => o.MapFrom(s => s.Alumno.NombreCompleto))
                .ForMember(d => d.AlergiasDetalladas, o => o.MapFrom(s => s.AlergiasRegistradas))
                .ForMember(d => d.Vacunas, o => o.MapFrom(s => s.Vacunas));

            CreateMap<Alergia, AlergiaDto>();
            CreateMap<Vacuna, VacunaDto>();

            CreateMap<CreateExpedienteDto, ExpedienteMedico>();
            CreateMap<UpdateExpedienteDto, ExpedienteMedico>();
        }
    }
}
