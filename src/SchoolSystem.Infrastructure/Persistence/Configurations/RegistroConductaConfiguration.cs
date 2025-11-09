using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Conducta;
using SchoolSystem.Domain.Enums.Conducta;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad RegistroConducta
    /// </summary>
    public class RegistroConductaConfiguration : IEntityTypeConfiguration<RegistroConducta>
    {
        public void Configure(EntityTypeBuilder<RegistroConducta> builder)
        {
            builder.HasQueryFilter(rc => !rc.Alumno.IsDeleted);

            // Nombre de tabla
            builder.ToTable("RegistrosConducta");

            // Clave primaria
            builder.HasKey(rc => rc.Id);

            // Propiedades requeridas
            builder.Property(rc => rc.EscuelaId)
                .IsRequired();

            builder.Property(rc => rc.AlumnoId)
                .IsRequired();

            builder.Property(rc => rc.MaestroId)
                .IsRequired();

            builder.Property(rc => rc.GrupoId)
                .IsRequired(false);

            builder.Property(rc => rc.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(rc => rc.Categoria)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(rc => rc.Gravedad)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(rc => rc.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(rc => rc.Descripcion)
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(rc => rc.FechaHoraIncidente)
                .IsRequired();

            builder.Property(rc => rc.Lugar)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(rc => rc.Puntos)
                .IsRequired();

            builder.Property(rc => rc.Testigos)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(rc => rc.EvidenciaUrls)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(rc => rc.SancionId)
                .IsRequired(false);

            builder.Property(rc => rc.AccionesTomadas)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(rc => rc.PadresNotificados)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(rc => rc.FechaNotificacionPadres)
                .IsRequired(false);

            builder.Property(rc => rc.MetodoNotificacion)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(rc => rc.RespuestaPadres)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(rc => rc.RequiereSeguimiento)
                .HasDefaultValue(false);

            builder.Property(rc => rc.FechaSeguimiento)
                .IsRequired(false);

            builder.Property(rc => rc.NotasSeguimiento)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(rc => rc.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(EstadoRegistroConducta.Activo);

            builder.Property(rc => rc.PeriodoId)
                .IsRequired(false);

            // Auditoría
            builder.Property(rc => rc.CreatedAt)
                .IsRequired();

            builder.Property(rc => rc.UpdatedAt)
                .IsRequired();

            builder.Property(rc => rc.CreatedBy)
                .IsRequired(false);

            builder.Property(rc => rc.UpdatedBy)
                .IsRequired(false);

            // Soft Delete
            builder.Property(rc => rc.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(rc => rc.DeletedAt)
                .IsRequired(false);

            builder.Property(rc => rc.DeletedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(rc => rc.Escuela)
                .WithMany()
                .HasForeignKey(rc => rc.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rc => rc.Alumno)
                .WithMany(a => a.RegistrosConducta)
                .HasForeignKey(rc => rc.AlumnoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rc => rc.Maestro)
                .WithMany()
                .HasForeignKey(rc => rc.MaestroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rc => rc.Grupo)
                .WithMany()
                .HasForeignKey(rc => rc.GrupoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(rc => rc.Sancion)
                .WithMany()
                .HasForeignKey(rc => rc.SancionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(rc => rc.EscuelaId)
                .HasDatabaseName("IX_RegistrosConducta_EscuelaId");

            builder.HasIndex(rc => rc.AlumnoId)
                .HasDatabaseName("IX_RegistrosConducta_AlumnoId");

            builder.HasIndex(rc => rc.MaestroId)
                .HasDatabaseName("IX_RegistrosConducta_MaestroId");

            builder.HasIndex(rc => rc.GrupoId)
                .HasDatabaseName("IX_RegistrosConducta_GrupoId");

            builder.HasIndex(rc => rc.Tipo)
                .HasDatabaseName("IX_RegistrosConducta_Tipo");

            builder.HasIndex(rc => rc.Categoria)
                .HasDatabaseName("IX_RegistrosConducta_Categoria");

            builder.HasIndex(rc => rc.Gravedad)
                .HasDatabaseName("IX_RegistrosConducta_Gravedad");

            builder.HasIndex(rc => rc.Estado)
                .HasDatabaseName("IX_RegistrosConducta_Estado");

            builder.HasIndex(rc => rc.FechaHoraIncidente)
                .HasDatabaseName("IX_RegistrosConducta_FechaHoraIncidente");

            builder.HasIndex(rc => rc.IsDeleted)
                .HasDatabaseName("IX_RegistrosConducta_IsDeleted");

            builder.HasIndex(rc => rc.RequiereSeguimiento)
                .HasDatabaseName("IX_RegistrosConducta_RequiereSeguimiento");

            builder.HasIndex(rc => rc.FechaSeguimiento)
                .HasDatabaseName("IX_RegistrosConducta_FechaSeguimiento");

            builder.HasIndex(rc => rc.SancionId)
                .HasDatabaseName("IX_RegistrosConducta_SancionId");

            builder.HasIndex(rc => rc.PeriodoId)
                .HasDatabaseName("IX_RegistrosConducta_PeriodoId");

            builder.HasIndex(rc => rc.PadresNotificados)
                .HasDatabaseName("IX_RegistrosConducta_PadresNotificados");

            // Índices compuestos para consultas frecuentes
            builder.HasIndex(rc => new { rc.AlumnoId, rc.FechaHoraIncidente })
                .HasDatabaseName("IX_RegistrosConducta_Alumno_Fecha");

            builder.HasIndex(rc => new { rc.EscuelaId, rc.Estado, rc.IsDeleted })
                .HasDatabaseName("IX_RegistrosConducta_Escuela_Estado_Deleted");

            builder.HasIndex(rc => new { rc.AlumnoId, rc.Tipo, rc.Estado })
                .HasDatabaseName("IX_RegistrosConducta_Alumno_Tipo_Estado");

            builder.HasIndex(rc => new { rc.MaestroId, rc.FechaHoraIncidente })
                .HasDatabaseName("IX_RegistrosConducta_Maestro_Fecha");

            builder.HasIndex(rc => new { rc.RequiereSeguimiento, rc.FechaSeguimiento })
                .HasDatabaseName("IX_RegistrosConducta_Seguimiento_Fecha");

            builder.HasIndex(rc => new { rc.Tipo, rc.Gravedad })
                .HasDatabaseName("IX_RegistrosConducta_Tipo_Gravedad");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(rc => rc.EsConductaPositiva);
            builder.Ignore(rc => rc.EsConductaNegativa);
            builder.Ignore(rc => rc.EsGrave);
            builder.Ignore(rc => rc.RequiereNotificacionInmediata);
            builder.Ignore(rc => rc.DiasDesdeIncidente);
            builder.Ignore(rc => rc.EstaPendienteSeguimiento);

            // Constraints
            builder.HasCheckConstraint("CK_RegistrosConducta_FechaSeguimiento",
                "`FechaSeguimiento` IS NULL OR `FechaSeguimiento` >= `FechaHoraIncidente`");
        }
    }
}