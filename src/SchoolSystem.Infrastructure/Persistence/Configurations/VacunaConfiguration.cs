using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Medico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Vacuna
    /// </summary>
    public class VacunaConfiguration : IEntityTypeConfiguration<Vacuna>
    {
        public void Configure(EntityTypeBuilder<Vacuna> builder)
        {
            // Nombre de tabla
            builder.ToTable("Vacunas");

            // Clave primaria
            builder.HasKey(v => v.Id);

            // Propiedades requeridas
            builder.Property(v => v.EscuelaId)
                .IsRequired();

            builder.Property(v => v.ExpedienteMedicoId)
                .IsRequired();

            builder.Property(v => v.NombreVacuna)
                .IsRequired()
                .HasMaxLength(100);

            // Información de aplicación
            builder.Property(v => v.Dosis)
                .HasMaxLength(50);

            builder.Property(v => v.FechaAplicacion)
                .IsRequired();

            builder.Property(v => v.FechaProximaDosis)
                .IsRequired(false);

            // Detalles del producto
            builder.Property(v => v.Lote)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(v => v.Marca)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(v => v.FechaCaducidad)
                .IsRequired(false);

            // Lugar de aplicación
            builder.Property(v => v.InstitucionAplicacion)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(v => v.PersonalAplicacion)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(v => v.LugarAnatomico)
                .HasMaxLength(100)
                .IsRequired(false);

            // Reacciones
            builder.Property(v => v.TuvoReacciones)
                .HasDefaultValue(false);

            builder.Property(v => v.DescripcionReacciones)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Documentación
            builder.Property(v => v.ComprobanteUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(v => v.Observaciones)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Relaciones
            builder.HasOne(v => v.ExpedienteMedico)
                .WithMany(em => em.Vacunas)
                .HasForeignKey(v => v.ExpedienteMedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(v => v.EscuelaId)
                .HasDatabaseName("IX_Vacunas_EscuelaId");

            builder.HasIndex(v => v.ExpedienteMedicoId)
                .HasDatabaseName("IX_Vacunas_ExpedienteMedicoId");

            builder.HasIndex(v => v.NombreVacuna)
                .HasDatabaseName("IX_Vacunas_NombreVacuna");

            builder.HasIndex(v => v.FechaAplicacion)
                .HasDatabaseName("IX_Vacunas_FechaAplicacion");

            builder.HasIndex(v => v.FechaProximaDosis)
                .HasDatabaseName("IX_Vacunas_FechaProximaDosis");

            builder.HasIndex(v => new { v.ExpedienteMedicoId, v.NombreVacuna, v.FechaAplicacion })
                .HasDatabaseName("IX_Vacunas_Expediente_Nombre_Fecha");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(v => v.TieneProximaDosis);
            builder.Ignore(v => v.ProximaDosisVencida);
            builder.Ignore(v => v.DiasHastaProximaDosis);
            builder.Ignore(v => v.ProximaDosisProxima);
            builder.Ignore(v => v.AniosDesdeAplicacion);
            builder.Ignore(v => v.TieneComprobante);
            builder.Ignore(v => v.LoteCaducado);

            // Constraints
            builder.HasCheckConstraint("CK_Vacunas_FechaProximaDosis",
                "`FechaProximaDosis` IS NULL OR `FechaProximaDosis` > `FechaAplicacion`");

            builder.HasCheckConstraint("CK_Vacunas_FechaCaducidad",
                "`FechaCaducidad` IS NULL OR `FechaCaducidad` >= `FechaAplicacion`");
        }
    }
}