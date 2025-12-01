using FluentValidation;
using SchoolSystem.Application.DTOs.Alumnos;
using SchoolSystem.Application.Validations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Validations.Alumnos
{
    public class UpdateAlumnoValidator : BasePersonaValidator<UpdateAlumnoDto>
    {
        public UpdateAlumnoValidator()
        {
            // Reglas comunes
            ApplyCommonRules();

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id del alumno es obligatorio.");

            RuleFor(x => x.Matricula)
                .NotEmpty().WithMessage("La matrícula es obligatoria.")
                .MaximumLength(50);

            RuleFor(x => x.CURP)
                .NotEmpty().WithMessage("La CURP es obligatoria.")
                .Length(18).WithMessage("El CURP debe tener 18 caracteres.");

            RuleFor(x => x.Genero)
                .IsInEnum().WithMessage("El género no es válido.");

            RuleFor(x => x.Direccion)
                .MaximumLength(300);

            RuleFor(x => x.FotoUrl)
                .MaximumLength(500);

            RuleFor(x => x.Estatus)
                .IsInEnum().WithMessage("El estatus no es válido.");

            // Datos médicos
            RuleFor(x => x.TipoSangre)
                .MaximumLength(50);

            RuleFor(x => x.Alergias)
                .MaximumLength(200);

            RuleFor(x => x.CondicionesMedicas)
                .MaximumLength(200);

            RuleFor(x => x.Medicamentos)
                .MaximumLength(200);

            // Contacto de emergencia
            RuleFor(x => x.ContactoEmergenciaNombre)
                .MaximumLength(200);

            RuleFor(x => x.ContactoEmergenciaTelefono)
                .MaximumLength(20);

            RuleFor(x => x.ContactoEmergenciaRelacion)
                .MaximumLength(100);
        }
    }
}
