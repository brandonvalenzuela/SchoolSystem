using FluentValidation;
using SchoolSystem.Application.DTOs.Materias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Materias
{
    public class CreateMateriaValidator : AbstractValidator<CreateMateriaDto>
    {
        public CreateMateriaValidator()
        {
            RuleFor(x => x.EscuelaId)
                .GreaterThan(0);

            RuleFor(x => x.Nombre)
                .NotEmpty().MaximumLength(100);

            RuleFor(x => x.Clave)
                .NotEmpty().MaximumLength(20);
        }
    }
}
