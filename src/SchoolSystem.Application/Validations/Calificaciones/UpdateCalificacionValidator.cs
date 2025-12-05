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

            RuleFor(x => x.MotivoModificacion)
                .NotEmpty().WithMessage("Se requiere el motivo de la modificación.")
                .MaximumLength(500);

            RuleFor(x => x.ModificadoPor).GreaterThan(0);

            // Campos OPCIONALES (Solo se validan si tienen valor)
            RuleFor(x => x.CalificacionNumerica)
                .InclusiveBetween(0, 10).WithMessage("La calificación debe estar entre 0 y 10.")
                .When(x => x.CalificacionNumerica.HasValue);

            RuleFor(x => x.Peso)
                .InclusiveBetween(0, 100)
                .When(x => x.Peso.HasValue);

            RuleFor(x => x.TipoEvaluacion)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.TipoEvaluacion));

            RuleFor(x => x.Observaciones)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Observaciones));

            RuleFor(x => x.Fortalezas)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Fortalezas));

            RuleFor(x => x.AreasOportunidad)
               .MaximumLength(500)
               .When(x => !string.IsNullOrEmpty(x.AreasOportunidad));

            RuleFor(x => x.Recomendaciones)
               .MaximumLength(500)
               .When(x => !string.IsNullOrEmpty(x.Recomendaciones));
        }
    }
}
