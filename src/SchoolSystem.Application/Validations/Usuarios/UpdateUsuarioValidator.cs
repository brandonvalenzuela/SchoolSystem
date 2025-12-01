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
    public class UpdateUsuarioValidator : BasePersonaValidator<UpdateUsuarioDto>
    {
        public UpdateUsuarioValidator()
        {
            ApplyCommonRules();

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id de usuario es obligatorio.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .Length(4, 100).WithMessage("El nombre de usuario debe tener entre 4 y 100 caracteres.");

            RuleFor(x => x.TelefonoEmergencia)
                .MaximumLength(20);

            RuleFor(x => x.FotoUrl)
                .MaximumLength(200);

            RuleFor(x => x.Rol)
                .IsInEnum().WithMessage("Rol de usuario no válido.");

            RuleFor(x => x.Activo)
                .NotNull();

            // Si permites cambiar contraseña en Update:
            // RuleFor(x => x.Password)
            //     .MinimumLength(8)
            //     .When(x => !string.IsNullOrWhiteSpace(x.Password));

            // Rol (si es enum)
            // RuleFor(x => x.Rol).IsInEnum();

            // Activo (si es bool?):
            // RuleFor(x => x.Activo).NotNull();
        }
    }
}