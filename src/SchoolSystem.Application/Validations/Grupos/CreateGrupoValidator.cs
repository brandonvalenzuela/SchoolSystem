using FluentValidation;
using SchoolSystem.Application.DTOs.Grupos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Grupos
{
    public class CreateGrupoValidator : AbstractValidator<CreateGrupoDto>
    {
        public CreateGrupoValidator()
        {
            RuleFor(x => x.EscuelaId)
                .GreaterThan(0);

            RuleFor(x => x.GradoId)
                .GreaterThan(0);

            RuleFor(x => x.Nombre)
                .NotEmpty().MaximumLength(50);

            RuleFor(x => x.Turno).IsInEnum();

            RuleFor(x => x.CapacidadMaxima)
                .GreaterThan(0);
        }
    }
}
