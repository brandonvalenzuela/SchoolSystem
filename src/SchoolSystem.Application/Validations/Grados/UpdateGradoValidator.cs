using FluentValidation;
using SchoolSystem.Application.DTOs.Grados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Grados
{
    public class UpdateGradoValidator : AbstractValidator<UpdateGradoDto>
    {
        public UpdateGradoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Nombre)
                .NotEmpty().MaximumLength(100);

            RuleFor(x => x.Orden).GreaterThanOrEqualTo(0);
        }
    }
}
