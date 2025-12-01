using FluentValidation;
using SchoolSystem.Application.DTOs.Asistencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Asistencias
{
    public class CreateAsistenciaValidator : AbstractValidator<CreateAsistenciaDto>
    {
        public CreateAsistenciaValidator()
        {
            RuleFor(x => x.AlumnoId)
                .GreaterThan(0);

            RuleFor(x => x.GrupoId)
                .GreaterThan(0);

            RuleFor(x => x.Fecha)
                .LessThanOrEqualTo(DateTime.Today);

            RuleFor(x => x.Estatus).IsInEnum(); // Presente, Ausente, Retardo, etc.

            RuleFor(x => x.Observaciones)
                .MaximumLength(300);
        }
    }
}
