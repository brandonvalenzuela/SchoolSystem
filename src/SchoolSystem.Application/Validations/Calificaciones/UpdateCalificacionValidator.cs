using FluentValidation;
using SchoolSystem.Application.DTOs.Calificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Calificaciones
{
    public class UpdateCalificacionValidator : AbstractValidator<UpdateCalificacionDto>
    {
        public UpdateCalificacionValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            // RuleFor(x => x.Periodo).NotEmpty().MaximumLength(50);

            RuleFor(x => x.CalificacionNumerica)
                .InclusiveBetween(0, 10);

            RuleFor(x => x.Observaciones)
                .MaximumLength(500);
        }
    }
}
