using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Finanzas;
using SchoolSystem.Domain.Enums.Finanzas;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Cargo
    /// </summary>
    public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            // Nombre de tabla
            builder.ToTable("Cargos");

            // Clave primaria
            builder.HasKey(c => c.Id);

            // Propiedades requeridas
            builder.Property(c => c.EscuelaId)
                .IsRequired();

            builder.Property(c => c.AlumnoId)
                .IsRequired();

            builder.Property(c => c.ConceptoPagoId)
                .IsRequired();

            builder.Property(c => c.CicloEscolar)
                .IsRequired()
                .HasMaxLength(20);

            // Montos
            builder.Property(c => c.Monto)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(c => c.Descuento)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(c => c.PorcentajeDescuento)
                .IsRequired(false)
                .HasColumnType("decimal(5,2)");

            builder.Property(c => c.MontoFinal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(c => c.Mora)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(c => c.MontoPagado)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(c => c.SaldoPendiente)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            // Fechas
            builder.Property(c => c.FechaCreacion)
                .IsRequired();

            builder.Property(c => c.FechaVencimiento)
                .IsRequired();

            builder.Property(c => c.FechaPagoCompleto)
                .IsRequired(false);

            // Estado
            builder.Property(c => c.Estatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10)
                .HasDefaultValue(EstatusCargo.Pendiente);

            builder.Property(c => c.Observaciones)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(c => c.MotivoCancelacion)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(c => c.FechaCancelacion)
                .IsRequired(false);

            builder.Property(c => c.CanceladoPorId)
                .IsRequired(false);

            // Metadata
            builder.Property(c => c.NumeroRecibo)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(c => c.ReferenciaExterna)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(c => c.GeneradoAutomaticamente)
                .HasDefaultValue(false);

            builder.Property(c => c.MesCorrespondiente)
                .IsRequired(false);

            // Auditoría
            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.CreatedBy)
                .IsRequired(false);

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(c => c.Alumno)
                .WithMany()
                .HasForeignKey(c => c.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(c => c.ConceptoPago)
                .WithMany(cp => cp.Cargos)
                .HasForeignKey(c => c.ConceptoPagoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.CanceladoPor)
                .WithMany()
                .HasForeignKey(c => c.CanceladoPorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Pagos)
                .WithOne(p => p.Cargo)
                .HasForeignKey(p => p.CargoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(c => c.EscuelaId)
                .HasDatabaseName("IX_Cargos_EscuelaId");

            builder.HasIndex(c => c.AlumnoId)
                .HasDatabaseName("IX_Cargos_AlumnoId");

            builder.HasIndex(c => c.ConceptoPagoId)
                .HasDatabaseName("IX_Cargos_ConceptoPagoId");

            builder.HasIndex(c => c.Estatus)
                .HasDatabaseName("IX_Cargos_Estatus");

            builder.HasIndex(c => new { c.AlumnoId, c.Estatus })
                .HasDatabaseName("IX_Cargos_Alumno_Estatus");

            builder.HasIndex(c => c.FechaVencimiento)
                .HasDatabaseName("IX_Cargos_FechaVencimiento");

            builder.HasIndex(c => new { c.AlumnoId, c.CicloEscolar })
                .HasDatabaseName("IX_Cargos_Alumno_Ciclo");

            builder.HasIndex(c => c.NumeroRecibo)
                .HasDatabaseName("IX_Cargos_NumeroRecibo");

            builder.HasIndex(c => new { c.EscuelaId, c.FechaVencimiento, c.Estatus })
                .HasDatabaseName("IX_Cargos_Escuela_Fecha_Estatus");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(c => c.EstaVencido);
            builder.Ignore(c => c.EstaPagado);
            builder.Ignore(c => c.PagoParcial);
            builder.Ignore(c => c.EstaCancelado);
            builder.Ignore(c => c.EstaPendiente);
            builder.Ignore(c => c.DiasRetraso);
            builder.Ignore(c => c.DiasParaVencer);
            builder.Ignore(c => c.MontoTotalConMora);
            builder.Ignore(c => c.PorcentajePagado);
            builder.Ignore(c => c.TotalPagos);
            builder.Ignore(c => c.TieneDescuento);
            builder.Ignore(c => c.TieneMora);

            // Constraints
            builder.HasCheckConstraint("CK_Cargos_Monto",
                "`Monto` >= 0");

            builder.HasCheckConstraint("CK_Cargos_Descuento",
                "`Descuento` >= 0 AND `Descuento` <= `Monto`");

            builder.HasCheckConstraint("CK_Cargos_MontoFinal",
                "`MontoFinal` >= 0");

            builder.HasCheckConstraint("CK_Cargos_Mora",
                "`Mora` >= 0");

            builder.HasCheckConstraint("CK_Cargos_MontoPagado",
                "`MontoPagado` >= 0");

            builder.HasCheckConstraint("CK_Cargos_SaldoPendiente",
                "`SaldoPendiente` >= 0");

            builder.HasCheckConstraint("CK_Cargos_FechaVencimiento",
                "`FechaVencimiento` >= `FechaCreacion`");
        }
    }
}