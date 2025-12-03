using FluentValidation;
using SchoolSystem.Application.DTOs.Asistencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Asistencias
{
    public class AsistenciaAlumnoValidator : AbstractValidator<AsistenciaAlumnoDto>
    {
        public AsistenciaAlumnoValidator()
        {
            RuleFor(x => x.AlumnoId).GreaterThan(0);
            RuleFor(x => x.Estatus).IsInEnum();
            RuleFor(x => x.MinutosRetardo).GreaterThanOrEqualTo(0);
        }
    }
}
