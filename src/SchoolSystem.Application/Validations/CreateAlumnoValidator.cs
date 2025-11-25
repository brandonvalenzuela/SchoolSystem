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
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email inválido")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.FechaNacimiento)
                .LessThan(DateTime.Now).WithMessage("La fecha de nacimiento debe ser en el pasado");
        }
    }
}
