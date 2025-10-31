using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Medico;
using SchoolSystem.Domain.Enums.Medico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Alergia
    /// </summary>
    public class AlergiaConfiguration : IEntityTypeConfiguration<Alergia>
    {
        public void Configure(EntityTypeBuilder<Alergia> builder)
        {
            // Nombre de tabla
            builder.ToTable("Alergias");

            // Clave primaria
            builder.HasKey(a => a.Id);

            // Propiedades requeridas
            builder.Property(a => a.EscuelaId)
                .IsRequired();

            builder.Property(a => a.ExpedienteMedicoId)
                .IsRequired();

            builder.Property(a => a.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(a => a.NombreAlergeno)
                .IsRequired()
                .HasMaxLength(200);

            // Gravedad y síntomas
            builder.Property(a => a.Gravedad)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(a => a.Sintomas)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            builder.Property(a => a.PuedeSerAnafilactica)
                .HasDefaultValue(false);

            // Diagnóstico
            builder.Property(a => a.FechaDiagnostico)
                .IsRequired(false);

            builder.Property(a => a.MedicoDiagnostico)
                .HasMaxLength(200);

            builder.Property(a => a.TipoPrueba)
                .HasMaxLength(200);

            // Tratamiento
            builder.Property(a => a.TratamientoRecomendado)
                .HasColumnType("LONGTEXT");

            builder.Property(a => a.MedicamentoEmergencia)
                .HasMaxLength(200);

            builder.Property(a => a.RequiereAutoinyector)
                .HasDefaultValue(false);

            builder.Property(a => a.InstruccionesEmergencia)
                .HasColumnType("LONGTEXT");

            // Control
            builder.Property(a => a.Activa)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(a => a.FechaSuperacion)
                .IsRequired(false);

            builder.Property(a => a.Observaciones)
                .HasColumnType("LONGTEXT");

            // Relaciones
            builder.HasOne(a => a.ExpedienteMedico)
                .WithMany(em => em.AlergiasRegistradas)
                .HasForeignKey(a => a.ExpedienteMedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(a => a.EscuelaId)
                .HasDatabaseName("IX_Alergias_EscuelaId");

            builder.HasIndex(a => a.ExpedienteMedicoId)
                .HasDatabaseName("IX_Alergias_ExpedienteMedicoId");

            builder.HasIndex(a => a.Tipo)
                .HasDatabaseName("IX_Alergias_Tipo");

            builder.HasIndex(a => a.Gravedad)
                .HasDatabaseName("IX_Alergias_Gravedad");

            builder.HasIndex(a => a.Activa)
                .HasDatabaseName("IX_Alergias_Activa");

            builder.HasIndex(a => a.PuedeSerAnafilactica)
                .HasDatabaseName("IX_Alergias_PuedeSerAnafilactica");

            builder.HasIndex(a => new { a.ExpedienteMedicoId, a.Activa })
                .HasDatabaseName("IX_Alergias_Expediente_Activa");

            builder.HasIndex(a => new { a.Tipo, a.Gravedad, a.Activa })
                .HasDatabaseName("IX_Alergias_Tipo_Gravedad_Activa");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(a => a.EsGrave);
            builder.Ignore(a => a.EsAlimentaria);
            builder.Ignore(a => a.EsMedicamento);
            builder.Ignore(a => a.EsCritica);
            builder.Ignore(a => a.AniosDesdeDignostico);
            builder.Ignore(a => a.DescripcionCompleta);

            // Constraints
            builder.HasCheckConstraint("CK_Alergias_FechaDiagnostico",
                "`FechaDiagnostico` IS NULL OR `FechaDiagnostico` <= GETDATE()");

            builder.HasCheckConstraint("CK_Alergias_FechaSuperacion",
                "`FechaSuperacion` IS NULL OR (`FechaSuperacion` >= `FechaDiagnostico` AND `Activa` = 0)");
        }
    }
}