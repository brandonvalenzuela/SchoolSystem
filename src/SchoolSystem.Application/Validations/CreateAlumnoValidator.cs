using FluentValidation;
using SchoolSystem.Application.DTOs.Alumnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validators
{
    public class CreateAlumnoValidator : AbstractValidator<CreateAlumnoDto>
    {
        public CreateAlumnoValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .MaximumLength(100).WithMessage("{PropertyName} no debe exceder 100 caracteres.");

            RuleFor(p => p.ApellidoPaterno)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");

            RuleFor(p => p.Matricula)
                .NotEmpty().WithMessage("La matrícula es obligatoria.")
                .Matches(@"^[A-Z0-9-]+$").WithMessage("La matrícula solo puede contener letras mayúsculas, números y guiones.");

            RuleFor(p => p.FechaNacimiento)
                .NotEmpty().WithMessage("Fecha de nacimiento requerida.")
                .LessThan(DateTime.Now.AddYears(-3)).WithMessage("El alumno debe tener al menos 3 años.");

            RuleFor(p => p.Email)
                .EmailAddress().WithMessage("Formato de email inválido.")
                .When(p => !string.IsNullOrEmpty(p.Email));

            RuleFor(p => p.EscuelaId)
                .GreaterThan(0).WithMessage("Debe especificar una escuela válida.");
        }
    }
}
