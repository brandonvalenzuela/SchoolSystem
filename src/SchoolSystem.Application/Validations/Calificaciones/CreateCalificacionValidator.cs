using FluentValidation;
using SchoolSystem.Application.DTOs.Calificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Calificaciones
{
    public class CreateCalificacionValidator : AbstractValidator<CreateCalificacionDto>
    {
        public CreateCalificacionValidator()
        {
            RuleFor(x => x.AlumnoId)
                .GreaterThan(0);

            RuleFor(x => x.MateriaId)
                .GreaterThan(0);

            // RuleFor(x => x.Periodo).NotEmpty().MaximumLength(50);

            RuleFor(x => x.CalificacionNumerica)
                .InclusiveBetween(0, 10);

            RuleFor(x => x.Observaciones)
                .MaximumLength(500);
        }
    }
}
