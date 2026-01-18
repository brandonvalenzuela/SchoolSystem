using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Finanzas;
using SchoolSystem.Domain.Enums.Finanzas;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad ConceptoPago
    /// </summary>
    public class ConceptoPagoConfiguration : IEntityTypeConfiguration<ConceptoPago>
    {
        public void Configure(EntityTypeBuilder<ConceptoPago> builder)
        {
            // Nombre de tabla
            builder.ToTable("ConceptosPago");

            // Clave primaria
            builder.HasKey(cp => cp.Id);

            // Propiedades requeridas
            builder.Property(cp => cp.EscuelaId)
                .IsRequired();

            builder.Property(cp => cp.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cp => cp.Descripcion)
                .IsRequired(false)
                .HasColumnType("LONGTEXT");

            builder.Property(cp => cp.MontoBase)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(cp => cp.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // Recurrencia
            builder.Property(cp => cp.Recurrente)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(cp => cp.Periodicidad)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Configuración
            builder.Property(cp => cp.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(cp => cp.Codigo)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(cp => cp.AplicaDescuentos)
                .HasDefaultValue(true);

            builder.Property(cp => cp.PorcentajeMaximoDescuento)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(cp => cp.TieneMora)
                .HasDefaultValue(false);

            builder.Property(cp => cp.PorcentajeMora)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(cp => cp.DiasGracia)
                .IsRequired(false);

            // Aplicabilidad
            builder.Property(cp => cp.NivelEducativoId)
                .IsRequired(false);

            builder.Property(cp => cp.GradoId)
                .IsRequired(false);

            builder.Property(cp => cp.CicloEscolar)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(x => x.CicloEscolarId)
                .IsRequired(false);

            builder.HasOne(x => x.Ciclo)
                .WithMany()
                .HasForeignKey(x => x.CicloEscolarId)
                .OnDelete(DeleteBehavior.Restrict);

            // Metadata
            builder.Property(cp => cp.CuentaContable)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(cp => cp.CategoriaFiscal)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(cp => cp.Notas)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Auditoría
            builder.Property(cp => cp.CreatedAt)
                .IsRequired();

            builder.Property(cp => cp.CreatedBy)
                .IsRequired(false);

            builder.Property(cp => cp.UpdatedAt)
                .IsRequired();

            builder.Property(cp => cp.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(cp => cp.NivelEducativo)
                .WithMany()
                .HasForeignKey(cp => cp.NivelEducativoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cp => cp.Grado)
                .WithMany()
                .HasForeignKey(cp => cp.GradoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cp => cp.Cargos)
                .WithOne(c => c.ConceptoPago)
                .HasForeignKey(c => c.ConceptoPagoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(cp => cp.EscuelaId)
                .HasDatabaseName("IX_ConceptosPago_EscuelaId");

            builder.HasIndex(cp => cp.Tipo)
                .HasDatabaseName("IX_ConceptosPago_Tipo");

            builder.HasIndex(cp => cp.Activo)
                .HasDatabaseName("IX_ConceptosPago_Activo");

            builder.HasIndex(cp => new { cp.EscuelaId, cp.Activo })
                .HasDatabaseName("IX_ConceptosPago_Escuela_Activo");

            builder.HasIndex(cp => cp.Codigo)
                .HasDatabaseName("IX_ConceptosPago_Codigo");

            builder.HasIndex(cp => cp.NivelEducativoId)
                .HasDatabaseName("IX_ConceptosPago_NivelEducativoId");

            builder.HasIndex(cp => cp.GradoId)
                .HasDatabaseName("IX_ConceptosPago_GradoId");

            builder.HasIndex(cp => new { cp.Recurrente, cp.Periodicidad })
                .HasDatabaseName("IX_ConceptosPago_Recurrente_Periodicidad");

            builder.HasIndex(x => new { x.EscuelaId, x.CicloEscolarId })
                .HasDatabaseName("IX_ConceptosPago_Escuela_CicloEscolarId");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(cp => cp.EsColegiatura);
            builder.Ignore(cp => cp.EsInscripcion);
            builder.Ignore(cp => cp.AplicaATodosLosNiveles);
            builder.Ignore(cp => cp.AplicaATodosLosGrados);
            builder.Ignore(cp => cp.NombreCompleto);

            // Constraints
            builder.HasCheckConstraint("CK_ConceptosPago_MontoBase",
                "`MontoBase` >= 0");

            builder.HasCheckConstraint("CK_ConceptosPago_PorcentajeDescuento",
                "`PorcentajeMaximoDescuento` IS NULL OR (`PorcentajeMaximoDescuento` >= 0 AND `PorcentajeMaximoDescuento` <= 100)");

            builder.HasCheckConstraint("CK_ConceptosPago_PorcentajeMora",
                "`PorcentajeMora` IS NULL OR `PorcentajeMora` >= 0");

            builder.HasCheckConstraint("CK_ConceptosPago_DiasGracia",
                "`DiasGracia` IS NULL OR `DiasGracia` >= 0");
        }
    }
}