using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Biblioteca;
using SchoolSystem.Domain.Enums.Biblioteca;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Prestamo
    /// </summary>
    public class PrestamoConfiguration : IEntityTypeConfiguration<Prestamo>
    {
        public void Configure(EntityTypeBuilder<Prestamo> builder)
        {
            // Nombre de tabla
            builder.ToTable("Prestamos");

            // Clave primaria
            builder.HasKey(p => p.Id);

            // Propiedades requeridas
            builder.Property(p => p.EscuelaId)
                .IsRequired();

            builder.Property(p => p.LibroId)
                .IsRequired();

            builder.Property(p => p.AlumnoId)
                .IsRequired(false);

            builder.Property(p => p.MaestroId)
                .IsRequired(false);

            builder.Property(p => p.RegistradoPorId)
                .IsRequired();

            // Fechas
            builder.Property(p => p.FechaPrestamo)
                .IsRequired();

            builder.Property(p => p.FechaDevolucionProgramada)
                .IsRequired();

            builder.Property(p => p.FechaDevolucionReal)
                .IsRequired(false);

            // Estado
            builder.Property(p => p.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10)
                .HasDefaultValue(EstadoPrestamo.Activo);

            builder.Property(p => p.Observaciones)
                .HasColumnType("LONGTEXT");

            builder.Property(p => p.ObservacionesDevolucion)
                .HasColumnType("LONGTEXT");

            // Multas
            builder.Property(p => p.MontoMulta)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(p => p.MultaPagada)
                .HasDefaultValue(false);

            builder.Property(p => p.FechaPagoMulta)
                .IsRequired(false);

            // Control de devolución
            builder.Property(p => p.DevueltoPorId)
                .IsRequired(false);

            builder.Property(p => p.CondicionDevolucion)
                .HasMaxLength(50);

            builder.Property(p => p.ReportadoExtraviado)
                .HasDefaultValue(false);

            builder.Property(p => p.FechaReporteExtravio)
                .IsRequired(false);

            builder.Property(p => p.ReportadoDaniado)
                .HasDefaultValue(false);

            // Renovaciones
            builder.Property(p => p.CantidadRenovaciones)
                .HasDefaultValue(0);

            builder.Property(p => p.FechaUltimaRenovacion)
                .IsRequired(false);

            // Metadata
            builder.Property(p => p.Folio)
                .HasMaxLength(50);

            builder.Property(p => p.PrestamoUrgente)
                .HasDefaultValue(false);

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
            builder.HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Alumno)
                .WithMany()
                .HasForeignKey(p => p.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Maestro)
                .WithMany()
                .HasForeignKey(p => p.MaestroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.RegistradoPor)
                .WithMany()
                .HasForeignKey(p => p.RegistradoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.DevueltoPor)
                .WithMany()
                .HasForeignKey(p => p.DevueltoPorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(p => p.EscuelaId)
                .HasDatabaseName("IX_Prestamos_EscuelaId");

            builder.HasIndex(p => p.LibroId)
                .HasDatabaseName("IX_Prestamos_LibroId");

            builder.HasIndex(p => p.AlumnoId)
                .HasDatabaseName("IX_Prestamos_AlumnoId");

            builder.HasIndex(p => p.MaestroId)
                .HasDatabaseName("IX_Prestamos_MaestroId");

            builder.HasIndex(p => p.Estado)
                .HasDatabaseName("IX_Prestamos_Estado");

            builder.HasIndex(p => p.FechaPrestamo)
                .HasDatabaseName("IX_Prestamos_FechaPrestamo");

            builder.HasIndex(p => p.FechaDevolucionProgramada)
                .HasDatabaseName("IX_Prestamos_FechaDevolucionProgramada");

            builder.HasIndex(p => new { p.Estado, p.FechaDevolucionProgramada })
                .HasDatabaseName("IX_Prestamos_Estado_FechaDevolucion");

            builder.HasIndex(p => p.Folio)
                .HasDatabaseName("IX_Prestamos_Folio");

            builder.HasIndex(p => new { p.AlumnoId, p.Estado })
                .HasDatabaseName("IX_Prestamos_Alumno_Estado");

            builder.HasIndex(p => new { p.EscuelaId, p.Estado, p.FechaPrestamo })
                .HasDatabaseName("IX_Prestamos_Escuela_Estado_Fecha");

            builder.HasIndex(p => p.MultaPagada)
                .HasDatabaseName("IX_Prestamos_MultaPagada");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(p => p.EstaActivo);
            builder.Ignore(p => p.EstaDevuelto);
            builder.Ignore(p => p.EstaVencido);
            builder.Ignore(p => p.DiasPrestamoProgramado);
            builder.Ignore(p => p.DiasRetraso);
            builder.Ignore(p => p.DiasHastaDevolucion);
            builder.Ignore(p => p.DiasPrestadoReal);
            builder.Ignore(p => p.DevueltoConRetraso);
            builder.Ignore(p => p.TieneMultaPendiente);
            builder.Ignore(p => p.EsPrestamoAlumno);
            builder.Ignore(p => p.EsPrestamoMaestro);
            builder.Ignore(p => p.NombreSolicitante);
            builder.Ignore(p => p.PuedeRenovarse);
            builder.Ignore(p => p.ProximoAVencer);

            // Constraints
            builder.HasCheckConstraint("CK_Prestamos_SolicitanteUnico",
                "(`AlumnoId` IS NOT NULL AND `MaestroId` IS NULL) OR (`AlumnoId` IS NULL AND `MaestroId` IS NOT NULL)");

            builder.HasCheckConstraint("CK_Prestamos_FechaDevolucionProgramada",
                "`FechaDevolucionProgramada` > `FechaPrestamo`");

            builder.HasCheckConstraint("CK_Prestamos_FechaDevolucionReal",
                "`FechaDevolucionReal` IS NULL OR `FechaDevolucionReal` >= `FechaPrestamo`");

            builder.HasCheckConstraint("CK_Prestamos_MontoMulta",
                "`MontoMulta` >= 0");

            builder.HasCheckConstraint("CK_Prestamos_CantidadRenovaciones",
                "`CantidadRenovaciones` >= 0");
        }
    }
}