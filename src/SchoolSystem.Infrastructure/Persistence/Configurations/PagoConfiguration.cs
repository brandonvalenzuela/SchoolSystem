using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Finanzas;
using SchoolSystem.Domain.Enums.Finanzas;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Pago
    /// </summary>
    public class PagoConfiguration : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> builder)
        {
            // Nombre de tabla
            builder.ToTable("Pagos");

            // Clave primaria
            builder.HasKey(p => p.Id);

            // Propiedades requeridas
            builder.Property(p => p.EscuelaId)
                .IsRequired();

            builder.Property(p => p.CargoId)
                .IsRequired();

            builder.Property(p => p.AlumnoId)
                .IsRequired();

            builder.Property(p => p.Monto)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            // Método de pago
            builder.Property(p => p.MetodoPago)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(p => p.Referencia)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(p => p.Banco)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(p => p.UltimosDigitosTarjeta)
                .HasMaxLength(4)
                .IsRequired(false);

            // Fechas
            builder.Property(p => p.FechaPago)
                .IsRequired();

            builder.Property(p => p.FechaAplicacion)
                .IsRequired(false);

            // Recibo
            builder.Property(p => p.FolioRecibo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.SerieRecibo)
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(p => p.ReciboUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            // Control
            builder.Property(p => p.RecibidoPorId)
                .IsRequired();

            builder.Property(p => p.Observaciones)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Cancelación
            builder.Property(p => p.Cancelado)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.FechaCancelacion)
                .IsRequired(false);

            builder.Property(p => p.MotivoCancelacion)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(p => p.CanceladoPorId)
                .IsRequired(false);

            // Facturación
            builder.Property(p => p.Facturado)
                .HasDefaultValue(false);

            builder.Property(p => p.UuidFactura)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(p => p.FacturaXmlUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(p => p.FacturaPdfUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(p => p.FechaFacturacion)
                .IsRequired(false);

            // Metadata
            builder.Property(p => p.ReferenciaExterna)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(p => p.DatosAdicionales)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(p => p.DireccionIp)
                .HasMaxLength(45)
                .IsRequired(false);

            // Auditoría
            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.CreatedBy)
                .IsRequired(false);

            builder.Property(p => p.UpdatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(p => p.Cargo)
                .WithMany(c => c.Pagos)
                .HasForeignKey(p => p.CargoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Alumno)
                .WithMany()
                .HasForeignKey(p => p.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(p => p.RecibidoPor)
                .WithMany()
                .HasForeignKey(p => p.RecibidoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CanceladoPor)
                .WithMany()
                .HasForeignKey(p => p.CanceladoPorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(p => p.EscuelaId)
                .HasDatabaseName("IX_Pagos_EscuelaId");

            builder.HasIndex(p => p.CargoId)
                .HasDatabaseName("IX_Pagos_CargoId");

            builder.HasIndex(p => p.AlumnoId)
                .HasDatabaseName("IX_Pagos_AlumnoId");

            builder.HasIndex(p => p.FechaPago)
                .HasDatabaseName("IX_Pagos_FechaPago");

            builder.HasIndex(p => p.FolioRecibo)
                .IsUnique()
                .HasDatabaseName("IX_Pagos_FolioRecibo_Unique");

            builder.HasIndex(p => p.MetodoPago)
                .HasDatabaseName("IX_Pagos_MetodoPago");

            builder.HasIndex(p => new { p.AlumnoId, p.FechaPago })
                .HasDatabaseName("IX_Pagos_Alumno_Fecha");

            builder.HasIndex(p => p.Cancelado)
                .HasDatabaseName("IX_Pagos_Cancelado");

            builder.HasIndex(p => p.Facturado)
                .HasDatabaseName("IX_Pagos_Facturado");

            builder.HasIndex(p => p.UuidFactura)
                .HasDatabaseName("IX_Pagos_UuidFactura");

            builder.HasIndex(p => new { p.EscuelaId, p.FechaPago, p.Cancelado })
                .HasDatabaseName("IX_Pagos_Escuela_Fecha_Cancelado");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(p => p.EstaActivo);
            builder.Ignore(p => p.EsConTarjeta);
            builder.Ignore(p => p.EsEfectivo);
            builder.Ignore(p => p.EsTransferencia);
            builder.Ignore(p => p.DiasDesdeElPago);
            builder.Ignore(p => p.TieneRecibo);
            builder.Ignore(p => p.PuedeCancelarse);
            builder.Ignore(p => p.NombreMetodoPago);

            // Constraints
            builder.HasCheckConstraint("CK_Pagos_Monto",
                "`Monto` > 0");

            builder.HasCheckConstraint("CK_Pagos_FechaAplicacion",
                "`FechaAplicacion` IS NULL OR `FechaAplicacion` >= `FechaPago`");

            builder.HasCheckConstraint("CK_Pagos_UltimosDigitos",
                "`UltimosDigitosTarjeta` IS NULL OR CHAR_LENGTH(`UltimosDigitosTarjeta`) = 4");
        }
    }
}