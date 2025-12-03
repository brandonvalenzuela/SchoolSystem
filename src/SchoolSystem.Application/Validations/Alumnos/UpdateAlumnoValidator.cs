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
    public class UpdateAlumnoValidator : BaseUpdatePersonaValidator<UpdateAlumnoDto>
    {
        public UpdateAlumnoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id del alumno es obligatorio.");

            // Reglas comunes
            ApplyCommonRules();

            // Datos Personales
            RuleFor(x => x.Matricula)
                .NotEmpty().WithMessage("La matrícula es obligatoria.")
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Matricula));

            RuleFor(x => x.CURP)
                .NotEmpty().WithMessage("La CURP es obligatoria.")
                .Length(18).WithMessage("El CURP debe tener 18 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.CURP));

            // Enums
            RuleFor(x => x.Genero)
                .IsInEnum().WithMessage("El género no es válido.");

            RuleFor(x => x.Estatus)
                .IsInEnum().WithMessage("El estatus no es válido.");

            // Otros
            RuleFor(x => x.Direccion).MaximumLength(300).When(x => !string.IsNullOrEmpty(x.Direccion));
            RuleFor(x => x.FotoUrl).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.FotoUrl));

            // Datos Médicos y Emergencia
            RuleFor(x => x.TipoSangre).MaximumLength(50).When(x => !string.IsNullOrEmpty(x.TipoSangre));
            RuleFor(x => x.Alergias).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Alergias));
            RuleFor(x => x.CondicionesMedicas).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.CondicionesMedicas));
            RuleFor(x => x.Medicamentos).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Medicamentos));
            RuleFor(x => x.ContactoEmergenciaNombre).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.ContactoEmergenciaNombre));
            RuleFor(x => x.ContactoEmergenciaTelefono).MaximumLength(20).When(x => !string.IsNullOrEmpty(x.ContactoEmergenciaTelefono));
            RuleFor(x => x.ContactoEmergenciaRelacion).MaximumLength(100).When(x => !string.IsNullOrEmpty(x.ContactoEmergenciaRelacion));
        }
    }
}
