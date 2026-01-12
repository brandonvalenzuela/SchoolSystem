using FluentValidation;
using SchoolSystem.Application.DTOs.Padres;
using SchoolSystem.Application.Validations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Padres
{
    public class CreatePadreValidator : BaseCreatePersonaValidator<CreatePadreDto>
    {
        public CreatePadreValidator()
        {
            ApplyCommonRules();

            RuleFor(x => x.EscuelaId)
               .GreaterThan(0).WithMessage("La escuela es obligatoria.");

            RuleFor(x => x.FechaNacimiento)
                .LessThan(DateTime.Now).WithMessage("La fecha de nacimiento no puede ser futura.")
                .When(x => x.FechaNacimiento.HasValue); // Solo valida si el usuario puso una fecha

            // Parentesco (si existe)
            // RuleFor(x => x.Parentesco)
            //     .NotEmpty().WithMessage("El parentesco es obligatorio.")
            //     .MaximumLength(50);
        }
    }
}
