using FluentValidation;
using SchoolSystem.Application.DTOs.Inscripciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Inscripciones
{
    public class UpdateInscripcionValidator : AbstractValidator<UpdateInscripcionDto>
    {
        public UpdateInscripcionValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.GrupoId)
                .GreaterThan(0);

            // Si tienes CicloId o CicloEscolarId:
            // RuleFor(x => x.CicloEscolarId).GreaterThan(0);

            RuleFor(x => x.Estatus).IsInEnum();
        }
    }
}
