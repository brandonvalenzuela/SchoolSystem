using FluentValidation;
using SchoolSystem.Application.DTOs.Usuarios;
using SchoolSystem.Application.Validations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Usuarios
{
    public class CreateUsuarioValidator : BasePersonaValidator<CreateUsuarioDto>
    {
        public CreateUsuarioValidator()
        {
            ApplyCommonRules();

            RuleFor(x => x.EscuelaId)
                .GreaterThan(0).WithMessage("Debe especificar una escuela válida.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .Length(4, 100).WithMessage("El nombre de usuario debe tener entre 4 y 100 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .Length(8, 100).WithMessage("La contraseña debe tener entre 8 y 100 caracteres.");


            RuleFor(x => x.TelefonoEmergencia)
                .MaximumLength(20);

            RuleFor(x => x.FotoUrl)
                .MaximumLength(200);

            RuleFor(x => x.Rol)
                .IsInEnum().WithMessage("Rol de usuario no válido.");
        }
    }



}
