using FluentValidation;
using SchoolSystem.Application.DTOs.Escuelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Escuelas
{
    public class CreateEscuelaValidator : AbstractValidator<CreateEscuelaDto>
    {
        public CreateEscuelaValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("El código es obligatorio.")
                .MaximumLength(20);

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la escuela es obligatorio.")
                .MaximumLength(150);

            RuleFor(x => x.RazonSocial)
                .MaximumLength(200);

            RuleFor(x => x.RFC)
                .NotEmpty().WithMessage("El RFC es obligatorio.")
                .MaximumLength(13);

            RuleFor(x => x.Direccion).MaximumLength(200);
            RuleFor(x => x.Ciudad).MaximumLength(100);
            RuleFor(x => x.Estado).MaximumLength(100);
            RuleFor(x => x.Pais).MaximumLength(100);
            RuleFor(x => x.CodigoPostal).MaximumLength(10);

            RuleFor(x => x.Telefono)
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .MaximumLength(200);

            // Plan / límites
            // RuleFor(x => x.TipoPlan).IsInEnum();

            RuleFor(x => x.MaxAlumnos)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaxMaestros)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.EspacioAlmacenamiento)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.FechaExpiracion)
                .GreaterThan(DateTime.Today)
                .When(x => x.FechaExpiracion.HasValue);
        }
    }
}
