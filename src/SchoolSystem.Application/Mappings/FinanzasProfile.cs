using AutoMapper;
using SchoolSystem.Application.DTOs.Finanzas;
using SchoolSystem.Domain.Entities.Finanzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class FinanzasProfile : Profile
    {
        public FinanzasProfile()
        {
            // Cargo -> CargoDto
            CreateMap<Cargo, CargoDto>()
                .ForMember(d => d.Concepto, o => o.MapFrom(s => s.ConceptoPago != null ? s.ConceptoPago.Nombre : "Cargo"))
                .ForMember(d => d.MontoTotal, o => o.MapFrom(s => s.MontoTotalConMora))
                .ForMember(d => d.Estatus, o => o.MapFrom(s => s.Estatus.ToString()))
                .ForMember(d => d.DiasRetraso, o => o.MapFrom(s => s.DiasRetraso));

            // Pago -> PagoDto
            CreateMap<Pago, PagoDto>()
                .ForMember(d => d.DescripcionCargo, o => o.MapFrom(s => s.Cargo != null && s.Cargo.ConceptoPago != null ? s.Cargo.ConceptoPago.Nombre : "Pago"))
                .ForMember(d => d.NombreAlumno, o => o.MapFrom(s => s.Alumno != null ? s.Alumno.NombreCompleto : "N/A"))
                .ForMember(d => d.MetodoPago, o => o.MapFrom(s => s.MetodoPago.ToString()));

            // CreatePagoDto -> Pago
            CreateMap<CreatePagoDto, Pago>();
        }
    }
}
