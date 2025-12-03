using FluentValidation;
using SchoolSystem.Application.DTOs.Padres;
using SchoolSystem.Application.Validations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Padres
{
    public class UpdatePadreValidator : BaseCreatePersonaValidator<UpdatePadreDto>
    {
        public UpdatePadreValidator()
        {
            ApplyCommonRules();

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id del padre/tutor es obligatorio.");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("El Id de usuario es obligatorio.");

            // Datos laborales / académicos (opcionales)
            RuleFor(x => x.Ocupacion)
                .MaximumLength(100);

            RuleFor(x => x.LugarTrabajo)
                .MaximumLength(200);

            RuleFor(x => x.TelefonoTrabajo)
                .MaximumLength(20);

            RuleFor(x => x.DireccionTrabajo)
                .MaximumLength(200);

            RuleFor(x => x.Puesto)
                .MaximumLength(100);

            RuleFor(x => x.NivelEstudios)
                .MaximumLength(100);

            RuleFor(x => x.Carrera)
                .MaximumLength(100);

            RuleFor(x => x.EstadoCivil)
                .MaximumLength(20);

            // Preferencias de notificación (si son bool? o bool)
            // Si son bool?:
            RuleFor(x => x.AceptaSMS).NotNull();
            RuleFor(x => x.AceptaEmail).NotNull();
            RuleFor(x => x.AceptaPush).NotNull();

            // Estatus
            RuleFor(x => x.Activo).NotNull();
        }
    }
}
