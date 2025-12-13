using FluentValidation;
using SchoolSystem.Application.DTOs.Escuelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Escuelas
{
    public class UpdateEscuelaValidator : AbstractValidator<UpdateEscuelaDto>
    {
        public UpdateEscuelaValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id de la escuela es obligatorio.");

            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("El código es obligatorio.")
                .MaximumLength(20);

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la escuela es obligatorio.")
                .MaximumLength(150);

            RuleFor(x => x.RazonSocial)
                .MaximumLength(200);

            RuleFor(x => x.RFC)
                // 1. Si lo escriben, debe tener entre 12 y 13 caracteres
                .Length(12, 13)
                // 2. Validación de formato real (Personas Morales o Físicas)
                // 3 letras (o 4), 6 números (fecha), 3 caracteres (homoclave)
                .Matches(@"^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A-Z\d])$")
                .WithMessage("El formato del RFC no es válido.")
                .When(x => !string.IsNullOrEmpty(x.RFC));

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

            // Plan
            // RuleFor(x => x.TipoPlan).IsInEnum();
            RuleFor(x => x.FechaExpiracion)
                .GreaterThan(DateTime.Today)
                .When(x => x.FechaExpiracion.HasValue);

            RuleFor(x => x.MaxAlumnos)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaxMaestros)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.EspacioAlmacenamiento)
                .GreaterThanOrEqualTo(0);

            // Activo (si existe)
            RuleFor(x => x.Activo).NotNull();
        }
    }
}
