using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Medico;
using SchoolSystem.Domain.Enums.Medico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Medicamento
    /// </summary>
    public class MedicamentoConfiguration : IEntityTypeConfiguration<Medicamento>
    {
        public void Configure(EntityTypeBuilder<Medicamento> builder)
        {
            // Nombre de tabla
            builder.ToTable("Medicamentos");

            // Clave primaria
            builder.HasKey(m => m.Id);

            // Propiedades requeridas
            builder.Property(m => m.EscuelaId)
                .IsRequired();

            builder.Property(m => m.ExpedienteMedicoId)
                .IsRequired();

            builder.Property(m => m.NombreMedicamento)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.NombreGenerico)
                .HasMaxLength(200)
                .IsRequired(false);

            // Prescripción
            builder.Property(m => m.Dosis)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Frecuencia)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Via)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(m => m.Indicacion)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            // Médico prescriptor
            builder.Property(m => m.MedicoPrescriptor)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(m => m.EspecialidadMedico)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(m => m.CedulaMedico)
                .HasMaxLength(50)
                .IsRequired(false);

            // Vigencia
            builder.Property(m => m.FechaInicio)
                .IsRequired();

            builder.Property(m => m.FechaFin)
                .IsRequired(false);

            builder.Property(m => m.TratamientoCronico)
                .HasDefaultValue(false);

            // Estado
            builder.Property(m => m.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(EstadoMedicamento.Activo);

            builder.Property(m => m.FechaSuspension)
                .IsRequired(false);

            builder.Property(m => m.MotivoSuspension)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Instrucciones
            builder.Property(m => m.InstruccionesEspeciales)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(m => m.Precauciones)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(m => m.EfectosSecundarios)
                .HasColumnType( "LONGTEXT")
                .IsRequired(false);

            // Control escolar
            builder.Property(m => m.AdministrarEnEscuela)
                .HasDefaultValue(false);

            builder.Property(m => m.HorarioEscolar)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(m => m.RequiereSupervision)
                .HasDefaultValue(false);

            builder.Property(m => m.PuedeAutoAdministrar)
                .HasDefaultValue(false);

            // Documentación
            builder.Property(m => m.RecetaUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(m => m.Observaciones)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Relaciones
            builder.HasOne(m => m.ExpedienteMedico)
                .WithMany(em => em.Medicamentos)
                .HasForeignKey(m => m.ExpedienteMedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(m => m.EscuelaId)
                .HasDatabaseName("IX_Medicamentos_EscuelaId");

            builder.HasIndex(m => m.ExpedienteMedicoId)
                .HasDatabaseName("IX_Medicamentos_ExpedienteMedicoId");

            builder.HasIndex(m => m.NombreMedicamento)
                .HasDatabaseName("IX_Medicamentos_NombreMedicamento");

            builder.HasIndex(m => m.Estado)
                .HasDatabaseName("IX_Medicamentos_Estado");

            builder.HasIndex(m => m.FechaInicio)
                .HasDatabaseName("IX_Medicamentos_FechaInicio");

            builder.HasIndex(m => m.FechaFin)
                .HasDatabaseName("IX_Medicamentos_FechaFin");

            builder.HasIndex(m => m.TratamientoCronico)
                .HasDatabaseName("IX_Medicamentos_TratamientoCronico");

            builder.HasIndex(m => m.AdministrarEnEscuela)
                .HasDatabaseName("IX_Medicamentos_AdministrarEnEscuela");

            builder.HasIndex(m => new { m.ExpedienteMedicoId, m.Estado })
                .HasDatabaseName("IX_Medicamentos_Expediente_Estado");

            builder.HasIndex(m => new { m.Estado, m.FechaFin })
                .HasDatabaseName("IX_Medicamentos_Estado_FechaFin");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(m => m.EstaActivo);
            builder.Ignore(m => m.EstaSuspendido);
            builder.Ignore(m => m.TratamientoFinalizado);
            builder.Ignore(m => m.EstaVigente);
            builder.Ignore(m => m.TratamientoVencido);
            builder.Ignore(m => m.DiasDesdeInicio);
            builder.Ignore(m => m.DiasHastaFin);
            builder.Ignore(m => m.DuracionTratamiento);
            builder.Ignore(m => m.TieneReceta);
            builder.Ignore(m => m.DescripcionCompleta);
            builder.Ignore(m => m.ProximoAVencer);

            // Constraints
            builder.HasCheckConstraint("CK_Medicamentos_FechaFin",
                "`FechaFin` IS NULL OR `FechaFin` >= `FechaInicio`");

            builder.HasCheckConstraint("CK_Medicamentos_FechaSuspension",
                "`FechaSuspension` IS NULL OR (`FechaSuspension` >= `FechaInicio` AND `Estado` = 'Suspendido')");
        }
    }
}