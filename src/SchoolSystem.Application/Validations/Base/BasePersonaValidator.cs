using FluentValidation;
using SchoolSystem.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Base
{
    public abstract class BasePersonaValidator <T> : AbstractValidator<T> where T : IPersonaDto
    {
        protected void ApplyCommonRules()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .MaximumLength(100).WithMessage("{PropertyName} no debe exceder 100 caracteres.");

            RuleFor(x => x.ApellidoPaterno)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .MaximumLength(100);

            RuleFor(x => x.ApellidoMaterno)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.")
                .When(p => !string.IsNullOrEmpty(p.Email))
                .MaximumLength(200);

            RuleFor(x => x.Telefono)
                .MaximumLength(20);

            RuleFor(p => p.FechaNacimiento)
                .NotEmpty().WithMessage("Fecha de nacimiento requerida.")
                .LessThan(DateTime.Now.AddYears(-3)).WithMessage("El alumno debe tener al menos 3 años.");
        }
    }
}
