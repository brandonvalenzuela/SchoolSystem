using FluentValidation;
using SchoolSystem.Application.DTOs.Asistencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Asistencias
{
    public class CreateAsistenciaMasivaValidator : AbstractValidator<CreateAsistenciaMasivaDto>
    {
        public CreateAsistenciaMasivaValidator()
        {
            RuleFor(x => x.GrupoId).GreaterThan(0);

            RuleFor(x => x.Fecha)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("No se puede tomar asistencia en fechas futuras.");

            RuleFor(x => x.Asistencias)
                .NotEmpty().WithMessage("La lista de alumnos no puede estar vacía.")
                .Must(x => x.Count > 0).WithMessage("Debe incluir al menos un alumno.");

            // Validación de los elementos hijos
            RuleForEach(x => x.Asistencias).SetValidator(new AsistenciaAlumnoValidator());
        }
    }
}
