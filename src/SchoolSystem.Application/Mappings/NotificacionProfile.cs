using AutoMapper;
using SchoolSystem.Application.DTOs.Comunicacion;
using SchoolSystem.Domain.Entities.Comunicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class NotificacionProfile : Profile
    {
        public NotificacionProfile()
        {
            CreateMap<Notificacion, NotificacionDto>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString()))
                .ForMember(dest => dest.Prioridad, opt => opt.MapFrom(src => src.Prioridad.ToString()));
        }
    }
}
