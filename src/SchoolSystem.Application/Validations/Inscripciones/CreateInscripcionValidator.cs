using FluentValidation;
using SchoolSystem.Application.DTOs.Inscripciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Inscripciones
{
    public class CreateInscripcionValidator : AbstractValidator<CreateInscripcionDto>
    {
        public CreateInscripcionValidator()
        {
            RuleFor(x => x.AlumnoId)
                .GreaterThan(0);

            RuleFor(x => x.GrupoId)
                .GreaterThan(0);

            // Si tienes CicloEscolarId / PeriodoId:
            // RuleFor(x => x.CicloEscolarId).GreaterThan(0);

            // RuleFor(x => x.Estatus).IsInEnum();
        }
    }
}
