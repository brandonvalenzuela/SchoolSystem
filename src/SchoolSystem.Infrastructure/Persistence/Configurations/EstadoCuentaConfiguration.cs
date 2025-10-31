using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Finanzas;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad EstadoCuenta
    /// </summary>
    public class EstadoCuentaConfiguration : IEntityTypeConfiguration<EstadoCuenta>
    {
        public void Configure(EntityTypeBuilder<EstadoCuenta> builder)
        {
            // Nombre de tabla
            builder.ToTable("EstadosCuenta");

            // Clave primaria
            builder.HasKey(ec => ec.Id);

            // Propiedades requeridas
            builder.Property(ec => ec.EscuelaId)
                .IsRequired();

            builder.Property(ec => ec.AlumnoId)
                .IsRequired();

            builder.Property(ec => ec.CicloEscolar)
                .IsRequired()
                .HasMaxLength(20);

            // Resumen financiero
            builder.Property(ec => ec.TotalCargos)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(ec => ec.TotalDescuentos)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(ec => ec.TotalMora)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(ec => ec.TotalPagos)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(ec => ec.SaldoPendiente)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(ec => ec.SaldoAFavor)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            // Contadores
            builder.Property(ec => ec.CargosPendientes)
                .HasDefaultValue(0);

            builder.Property(ec => ec.CargosPagados)
                .HasDefaultValue(0);

            builder.Property(ec => ec.CargosVencidos)
                .HasDefaultValue(0);

            builder.Property(ec => ec.CargosParciales)
                .HasDefaultValue(0);

            builder.Property(ec => ec.TotalCargosCantidad)
                .HasDefaultValue(0);

            builder.Property(ec => ec.TotalPagosCantidad)
                .HasDefaultValue(0);

            // Estado
            builder.Property(ec => ec.TieneAdeudos)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(ec => ec.TieneCargosVencidos)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(ec => ec.AlCorriente)
                .IsRequired()
                .HasDefaultValue(true);

            // Fechas
            builder.Property(ec => ec.FechaUltimoCargo)
                .IsRequired(false);

            builder.Property(ec => ec.FechaUltimoPago)
                .IsRequired(false);

            builder.Property(ec => ec.FechaActualizacion)
                .IsRequired();

            builder.Property(ec => ec.FechaProximoVencimiento)
                .IsRequired(false);

            // Metadata
            builder.Property(ec => ec.Observaciones)
                .HasColumnType("LONGTEXT");

            builder.Property(ec => ec.RequiereAtencion)
                .HasDefaultValue(false);

            builder.Property(ec => ec.NotasAtencion)
                .HasColumnType("LONGTEXT");

            // Auditoría
            builder.Property(ec => ec.CreatedAt)
                .IsRequired();

            builder.Property(ec => ec.CreatedBy)
                .IsRequired(false);

            builder.Property(ec => ec.UpdatedAt)
                .IsRequired();

            builder.Property(ec => ec.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(ec => ec.Alumno)
                .WithMany()
                .HasForeignKey(ec => ec.AlumnoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(ec => ec.EscuelaId)
                .HasDatabaseName("IX_EstadosCuenta_EscuelaId");

            builder.HasIndex(ec => ec.AlumnoId)
                .HasDatabaseName("IX_EstadosCuenta_AlumnoId");

            // Índice único: Un alumno solo puede tener un estado de cuenta por ciclo
            builder.HasIndex(ec => new { ec.AlumnoId, ec.CicloEscolar })
                .IsUnique()
                .HasDatabaseName("IX_EstadosCuenta_Alumno_Ciclo_Unique");

            builder.HasIndex(ec => ec.TieneAdeudos)
                .HasDatabaseName("IX_EstadosCuenta_TieneAdeudos");

            builder.HasIndex(ec => ec.TieneCargosVencidos)
                .HasDatabaseName("IX_EstadosCuenta_TieneCargosVencidos");

            builder.HasIndex(ec => ec.AlCorriente)
                .HasDatabaseName("IX_EstadosCuenta_AlCorriente");

            builder.HasIndex(ec => ec.RequiereAtencion)
                .HasDatabaseName("IX_EstadosCuenta_RequiereAtencion");

            builder.HasIndex(ec => ec.FechaActualizacion)
                .HasDatabaseName("IX_EstadosCuenta_FechaActualizacion");

            builder.HasIndex(ec => new { ec.EscuelaId, ec.CicloEscolar, ec.TieneAdeudos })
                .HasDatabaseName("IX_EstadosCuenta_Escuela_Ciclo_Adeudos");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(ec => ec.TotalAPagar);
            builder.Ignore(ec => ec.PorcentajePagado);
            builder.Ignore(ec => ec.PorcentajeMorosidad);
            builder.Ignore(ec => ec.DiasDesdeUltimoPago);
            builder.Ignore(ec => ec.DiasHastaProximoVencimiento);
            builder.Ignore(ec => ec.TieneVencimientoCercano);
            builder.Ignore(ec => ec.PromedioPago);
            builder.Ignore(ec => ec.PromedioCargo);
            builder.Ignore(ec => ec.EstadoGeneral);

            // Constraints
            builder.HasCheckConstraint("CK_EstadosCuenta_TotalCargos",
                "`TotalCargos` >= 0");

            builder.HasCheckConstraint("CK_EstadosCuenta_TotalDescuentos",
                "`TotalDescuentos` >= 0 AND `TotalDescuentos` <= `TotalCargos`");

            builder.HasCheckConstraint("CK_EstadosCuenta_TotalPagos",
                "`TotalPagos` >= 0");

            builder.HasCheckConstraint("CK_EstadosCuenta_SaldoPendiente",
                "`SaldoPendiente` >= 0");

            builder.HasCheckConstraint("CK_EstadosCuenta_SaldoAFavor",
                "`SaldoAFavor` >= 0");

            builder.HasCheckConstraint("CK_EstadosCuenta_Contadores",
                "`CargosPendientes` >= 0 AND `CargosPagados` >= 0 AND `CargosVencidos` >= 0 AND `CargosParciales` >= 0");
        }
    }
}