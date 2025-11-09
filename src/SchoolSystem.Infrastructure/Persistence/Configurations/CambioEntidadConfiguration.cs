using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Auditoria;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad CambioEntidad
    /// </summary>
    public class CambioEntidadConfiguration : IEntityTypeConfiguration<CambioEntidad>
    {
        public void Configure(EntityTypeBuilder<CambioEntidad> builder)
        {
            // Nombre de tabla
            builder.ToTable("CambiosEntidad");

            // Clave primaria
            builder.HasKey(ce => ce.Id);

            // Propiedades requeridas
            builder.Property(ce => ce.EscuelaId)
                .IsRequired(false);

            builder.Property(ce => ce.LogAuditoriaId)
                .IsRequired(false);

            builder.Property(ce => ce.NombreEntidad)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ce => ce.EntidadId)
                .IsRequired();

            // Campo modificado
            builder.Property(ce => ce.NombreCampo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ce => ce.NombreDescriptivo)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(ce => ce.TipoDato)
                .IsRequired()
                .HasMaxLength(50);

            // Valores
            builder.Property(ce => ce.ValorAnterior)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(ce => ce.ValorNuevo)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(ce => ce.ValorAnteriorFormateado)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(ce => ce.ValorNuevoFormateado)
                .HasMaxLength(500)
                .IsRequired(false);

            // Usuario y fecha
            builder.Property(ce => ce.UsuarioId)
                .IsRequired(false);

            builder.Property(ce => ce.NombreUsuario)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(ce => ce.FechaCambio)
                .IsRequired();

            // Metadata
            builder.Property(ce => ce.EsCampoSensible)
                .HasDefaultValue(false);

            builder.Property(ce => ce.Categoria)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(ce => ce.Etiquetas)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(ce => ce.Notas)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Relaciones
            builder.HasOne(ce => ce.LogAuditoria)
                .WithMany()
                .HasForeignKey(ce => ce.LogAuditoriaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ce => ce.Usuario)
                .WithMany()
                .HasForeignKey(ce => ce.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(ce => ce.EscuelaId)
                .HasDatabaseName("IX_CambiosEntidad_EscuelaId");

            builder.HasIndex(ce => ce.LogAuditoriaId)
                .HasDatabaseName("IX_CambiosEntidad_LogAuditoriaId");

            builder.HasIndex(ce => ce.NombreEntidad)
                .HasDatabaseName("IX_CambiosEntidad_NombreEntidad");

            builder.HasIndex(ce => ce.EntidadId)
                .HasDatabaseName("IX_CambiosEntidad_EntidadId");

            builder.HasIndex(ce => new { ce.NombreEntidad, ce.EntidadId })
                .HasDatabaseName("IX_CambiosEntidad_Entidad_EntidadId");

            builder.HasIndex(ce => ce.NombreCampo)
                .HasDatabaseName("IX_CambiosEntidad_NombreCampo");

            builder.HasIndex(ce => ce.FechaCambio)
                .HasDatabaseName("IX_CambiosEntidad_FechaCambio");

            builder.HasIndex(ce => ce.UsuarioId)
                .HasDatabaseName("IX_CambiosEntidad_UsuarioId");

            builder.HasIndex(ce => ce.EsCampoSensible)
                .HasDatabaseName("IX_CambiosEntidad_EsCampoSensible");

            builder.HasIndex(ce => new { ce.NombreEntidad, ce.EntidadId, ce.FechaCambio })
                .HasDatabaseName("IX_CambiosEntidad_Entidad_EntidadId_Fecha");

            builder.HasIndex(ce => new { ce.NombreEntidad, ce.NombreCampo, ce.FechaCambio })
                .HasDatabaseName("IX_CambiosEntidad_Entidad_Campo_Fecha");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(ce => ce.ValorCambio);
            builder.Ignore(ce => ce.EsNuloANoNulo);
            builder.Ignore(ce => ce.EsNoNuloANulo);
            builder.Ignore(ce => ce.AmbosNulos);
            builder.Ignore(ce => ce.EsCampoNumerico);
            builder.Ignore(ce => ce.EsCampoFecha);
            builder.Ignore(ce => ce.EsCampoBooleano);
            builder.Ignore(ce => ce.DiferenciaNumerica);
            builder.Ignore(ce => ce.PorcentajeCambio);
            builder.Ignore(ce => ce.DescripcionCambio);
            builder.Ignore(ce => ce.DiasDesdeElCambio);

        }
    }
}