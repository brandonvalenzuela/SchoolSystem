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
    public class CreateMaestroValidator : BasePersonaValidator<CreateMaestroDto>
    {
        public CreateMaestroValidator()
        {
            ApplyCommonRules();

            RuleFor(x => x.CedulaProfesional)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Especialidad)
                .MaximumLength(100);
        }
    }
}
