using FluentValidation;
using SchoolSystem.Application.DTOs.Maestros;
using SchoolSystem.Application.Validations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Maestros
{
    public class UpdateMaestroValidator : BaseCreatePersonaValidator<UpdateMaestroDto>
    {
        public UpdateMaestroValidator()
        {
            ApplyCommonRules();

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id del maestro es obligatorio.");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("El Id de usuario es obligatorio.");

            RuleFor(x => x.NumeroEmpleado)
                .NotEmpty().WithMessage("El número de empleado es obligatorio.")
                .MaximumLength(20);

            RuleFor(x => x.FechaIngreso)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de ingreso no puede ser futura.");

            RuleFor(x => x.FechaBaja)
                .GreaterThanOrEqualTo(x => x.FechaIngreso)
                .When(x => x.FechaBaja.HasValue)
                .WithMessage("La fecha de baja no puede ser anterior a la de ingreso.");

            RuleFor(x => x.TipoContrato)
                .NotEmpty().WithMessage("El tipo de contrato es obligatorio.");

            RuleFor(x => x.CedulaProfesional)
                .MaximumLength(20);

            RuleFor(x => x.Especialidad)
                .MaximumLength(100);

            RuleFor(x => x.TituloAcademico)
                .MaximumLength(100);

            RuleFor(x => x.Universidad)
                .MaximumLength(150);

            RuleFor(x => x.AñoGraduacion)
                .InclusiveBetween(1900, DateTime.Now.Year)
                .When(x => x.AñoGraduacion.HasValue);

            RuleFor(x => x.AñosExperiencia)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Salario)
                .GreaterThan(0)
                .When(x => x.Salario.HasValue);

            // Estatus (activo/inactivo, enum, etc.)
            RuleFor(x => x.Estatus).NotNull();
        }
    }
}
