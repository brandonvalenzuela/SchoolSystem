using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Medico;
using SchoolSystem.Domain.Enums.Medico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad ExpedienteMedico
    /// </summary>
    public class ExpedienteMedicoConfiguration : IEntityTypeConfiguration<ExpedienteMedico>
    {
        public void Configure(EntityTypeBuilder<ExpedienteMedico> builder)
        {
            // Nombre de tabla
            builder.ToTable("ExpedientesMedicos");

            // Clave primaria
            builder.HasKey(em => em.Id);

            // Propiedades requeridas
            builder.Property(em => em.EscuelaId)
                .IsRequired();

            builder.Property(em => em.AlumnoId)
                .IsRequired();

            // Información médica básica
            builder.Property(em => em.TipoSangre)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(em => em.Peso)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(em => em.Estatura)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(em => em.IMC)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            // Condiciones médicas
            builder.Property(em => em.Alergias)
                .HasColumnType("LONGTEXT");

            builder.Property(em => em.CondicionesMedicas)
                .HasColumnType("LONGTEXT");

            builder.Property(em => em.MedicamentosRegulares)
                .HasColumnType("LONGTEXT");

            builder.Property(em => em.Restricciones)
                .HasColumnType("LONGTEXT");

            builder.Property(em => em.RequiereAtencionEspecial)
                .HasDefaultValue(false);

            builder.Property(em => em.DetallesAtencionEspecial)
                .HasColumnType("LONGTEXT");

            // Contacto de emergencia
            builder.Property(em => em.ContactoEmergenciaNombre)
                .HasMaxLength(200);

            builder.Property(em => em.ContactoEmergenciaTelefono)
                .HasMaxLength(20);

            builder.Property(em => em.ContactoEmergenciaTelefonoAlt)
                .HasMaxLength(20);

            builder.Property(em => em.ContactoEmergenciaParentesco)
                .HasMaxLength(50);

            // Seguro médico
            builder.Property(em => em.TieneSeguro)
                .HasDefaultValue(false);

            builder.Property(em => em.SeguroNombre)
                .HasMaxLength(200);

            builder.Property(em => em.SeguroNumeroPoliza)
                .HasMaxLength(100);

            builder.Property(em => em.SeguroVigencia)
                .IsRequired(false);

            builder.Property(em => em.SeguroTelefono)
                .HasMaxLength(20);

            // Médico particular
            builder.Property(em => em.MedicoNombre)
                .HasMaxLength(200);

            builder.Property(em => em.MedicoEspecialidad)
                .HasMaxLength(100);

            builder.Property(em => em.MedicoTelefono)
                .HasMaxLength(20);

            builder.Property(em => em.MedicoDireccion)
                .HasMaxLength(300);

            // Vacunación
            builder.Property(em => em.VacunacionCompleta)
                .HasDefaultValue(false);

            builder.Property(em => em.VacunacionObservaciones)
                .HasColumnType("LONGTEXT");

            // Control y auditoría
            builder.Property(em => em.FechaUltimaActualizacion)
                .IsRequired(false);

            builder.Property(em => em.FechaUltimaRevision)
                .IsRequired(false);

            builder.Property(em => em.Observaciones)
                .HasColumnType("LONGTEXT");

            builder.Property(em => em.ExpedienteCompleto)
                .HasDefaultValue(false);

            builder.Property(em => em.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Auditoría
            builder.Property(em => em.CreatedAt)
                .IsRequired();

            builder.Property(em => em.CreatedBy)
                .IsRequired(false);

            builder.Property(em => em.UpdatedAt)
                .IsRequired();

            builder.Property(em => em.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(em => em.Alumno)
                .WithMany()
                .HasForeignKey(em => em.AlumnoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(em => em.Vacunas)
                .WithOne(v => v.ExpedienteMedico)
                .HasForeignKey(v => v.ExpedienteMedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(em => em.AlergiasRegistradas)
                .WithOne(a => a.ExpedienteMedico)
                .HasForeignKey(a => a.ExpedienteMedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(em => em.Medicamentos)
                .WithOne(m => m.ExpedienteMedico)
                .HasForeignKey(m => m.ExpedienteMedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(em => em.HistorialMedico)
                .WithOne(hm => hm.ExpedienteMedico)
                .HasForeignKey(hm => hm.ExpedienteMedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(em => em.EscuelaId)
                .HasDatabaseName("IX_ExpedientesMedicos_EscuelaId");

            // Índice único: Un alumno solo puede tener un expediente médico
            builder.HasIndex(em => em.AlumnoId)
                .IsUnique()
                .HasDatabaseName("IX_ExpedientesMedicos_AlumnoId_Unique");

            builder.HasIndex(em => em.Activo)
                .HasDatabaseName("IX_ExpedientesMedicos_Activo");

            builder.HasIndex(em => em.RequiereAtencionEspecial)
                .HasDatabaseName("IX_ExpedientesMedicos_RequiereAtencionEspecial");

            builder.HasIndex(em => em.TieneSeguro)
                .HasDatabaseName("IX_ExpedientesMedicos_TieneSeguro");

            builder.HasIndex(em => em.SeguroVigencia)
                .HasDatabaseName("IX_ExpedientesMedicos_SeguroVigencia");

            builder.HasIndex(em => em.FechaUltimaActualizacion)
                .HasDatabaseName("IX_ExpedientesMedicos_FechaUltimaActualizacion");

            builder.HasIndex(em => new { em.EscuelaId, em.Activo })
                .HasDatabaseName("IX_ExpedientesMedicos_Escuela_Activo");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(em => em.TieneAlergias);
            builder.Ignore(em => em.TieneCondicionesMedicas);
            builder.Ignore(em => em.TomaMedicamentos);
            builder.Ignore(em => em.TieneContactoEmergencia);
            builder.Ignore(em => em.SeguroVigente);
            builder.Ignore(em => em.DiasHastaVencimientoSeguro);
            builder.Ignore(em => em.SeguroProximoVencer);
            builder.Ignore(em => em.ClasificacionIMC);
            builder.Ignore(em => em.DiasSinActualizar);
            builder.Ignore(em => em.NecesitaActualizacion);
            builder.Ignore(em => em.TotalVacunas);
            builder.Ignore(em => em.TotalAlergias);

            // Constraints
            builder.HasCheckConstraint("CK_ExpedientesMedicos_Peso",
                "`Peso` IS NULL OR `Peso` > 0");

            builder.HasCheckConstraint("CK_ExpedientesMedicos_Estatura",
                "`Estatura` IS NULL OR `Estatura` > 0");

            builder.HasCheckConstraint("CK_ExpedientesMedicos_IMC",
                "`IMC` IS NULL OR `IMC` > 0");
        }
    }
}