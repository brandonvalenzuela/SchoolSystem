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

            RuleFor(x => x.Ocupacion)
                .MaximumLength(100);

            RuleFor(x => x.LugarTrabajo)
                .MaximumLength(200);

            RuleFor(x => x.TelefonoTrabajo)
                .MaximumLength(20);

            RuleFor(x => x.DireccionTrabajo)
                .MaximumLength(200);

            RuleFor(x => x.Puesto)
                .MaximumLength(100);

            RuleFor(x => x.NivelEstudios)
                .MaximumLength(100);

            RuleFor(x => x.Carrera)
                .MaximumLength(100);

            RuleFor(x => x.EstadoCivil)
                .MaximumLength(20);

            // Parentesco (si existe)
            // RuleFor(x => x.Parentesco)
            //     .NotEmpty().WithMessage("El parentesco es obligatorio.")
            //     .MaximumLength(50);
        }
    }
}
