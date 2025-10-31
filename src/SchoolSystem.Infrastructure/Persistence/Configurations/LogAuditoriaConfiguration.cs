using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Auditoria;
using SchoolSystem.Domain.Enums.Auditoria;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad LogAuditoria
    /// </summary>
    public class LogAuditoriaConfiguration : IEntityTypeConfiguration<LogAuditoria>
    {
        public void Configure(EntityTypeBuilder<LogAuditoria> builder)
        {
            // Nombre de tabla
            builder.ToTable("LogsAuditoria");

            // Clave primaria
            builder.HasKey(la => la.Id);

            // Propiedades requeridas
            builder.Property(la => la.EscuelaId)
                .IsRequired(false);

            builder.Property(la => la.UsuarioId)
                .IsRequired(false);

            builder.Property(la => la.NombreUsuario)
                .HasMaxLength(200);

            builder.Property(la => la.EmailUsuario)
                .HasMaxLength(200);

            // Acción
            builder.Property(la => la.TipoAccion)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(la => la.Descripcion)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            builder.Property(la => la.FechaHora)
                .IsRequired();

            // Entidad afectada
            builder.Property(la => la.EntidadAfectada)
                .HasMaxLength(100);

            builder.Property(la => la.EntidadAfectadaId)
                .IsRequired(false);

            builder.Property(la => la.TipoEntidad)
                .HasMaxLength(100);

            // Cambios
            builder.Property(la => la.ValoresAnteriores)
                .HasColumnType("LONGTEXT");

            builder.Property(la => la.ValoresNuevos)
                .HasColumnType("LONGTEXT");

            builder.Property(la => la.CamposModificados)
                .HasMaxLength(500);

            // Información técnica
            builder.Property(la => la.DireccionIP)
                .HasMaxLength(45);

            builder.Property(la => la.UserAgent)
                .HasMaxLength(500);

            builder.Property(la => la.Navegador)
                .HasMaxLength(100);

            builder.Property(la => la.SistemaOperativo)
                .HasMaxLength(100);

            builder.Property(la => la.Dispositivo)
                .HasMaxLength(50);

            // Resultado
            builder.Property(la => la.Exitoso)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(la => la.CodigoResultado)
                .IsRequired(false);

            builder.Property(la => la.MensajeError)
                .HasColumnType("LONGTEXT");

            builder.Property(la => la.StackTrace)
                .HasColumnType("LONGTEXT");

            // Rendimiento
            builder.Property(la => la.DuracionMs)
                .IsRequired(false);

            // Módulo y funcionalidad
            builder.Property(la => la.Modulo)
                .HasMaxLength(100);

            builder.Property(la => la.Funcionalidad)
                .HasMaxLength(200);

            builder.Property(la => la.Controlador)
                .HasMaxLength(200);

            builder.Property(la => la.Metodo)
                .HasMaxLength(200);

            // Metadata
            builder.Property(la => la.DatosAdicionales)
                .HasColumnType("LONGTEXT");

            builder.Property(la => la.Severidad)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(la => la.Tags)
                .HasMaxLength(500);

            // Relaciones
            builder.HasOne(la => la.Usuario)
                .WithMany()
                .HasForeignKey(la => la.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(la => la.EscuelaId)
                .HasDatabaseName("IX_LogsAuditoria_EscuelaId");

            builder.HasIndex(la => la.UsuarioId)
                .HasDatabaseName("IX_LogsAuditoria_UsuarioId");

            builder.HasIndex(la => la.TipoAccion)
                .HasDatabaseName("IX_LogsAuditoria_TipoAccion");

            builder.HasIndex(la => la.FechaHora)
                .HasDatabaseName("IX_LogsAuditoria_FechaHora");

            builder.HasIndex(la => la.EntidadAfectada)
                .HasDatabaseName("IX_LogsAuditoria_EntidadAfectada");

            builder.HasIndex(la => new { la.EntidadAfectada, la.EntidadAfectadaId })
                .HasDatabaseName("IX_LogsAuditoria_Entidad_EntidadId");

            builder.HasIndex(la => la.Exitoso)
                .HasDatabaseName("IX_LogsAuditoria_Exitoso");

            builder.HasIndex(la => la.DireccionIP)
                .HasDatabaseName("IX_LogsAuditoria_DireccionIP");

            builder.HasIndex(la => la.Modulo)
                .HasDatabaseName("IX_LogsAuditoria_Modulo");

            builder.HasIndex(la => la.Severidad)
                .HasDatabaseName("IX_LogsAuditoria_Severidad");

            builder.HasIndex(la => new { la.EscuelaId, la.FechaHora })
                .HasDatabaseName("IX_LogsAuditoria_Escuela_Fecha");

            builder.HasIndex(la => new { la.TipoAccion, la.FechaHora })
                .HasDatabaseName("IX_LogsAuditoria_TipoAccion_Fecha");

            builder.HasIndex(la => new { la.EscuelaId, la.TipoAccion, la.FechaHora })
                .HasDatabaseName("IX_LogsAuditoria_Escuela_Tipo_Fecha");

            builder.HasIndex(la => new { la.UsuarioId, la.FechaHora })
                .HasDatabaseName("IX_LogsAuditoria_Usuario_Fecha");

            builder.HasIndex(la => new { la.Exitoso, la.FechaHora })
                .HasDatabaseName("IX_LogsAuditoria_Exitoso_Fecha");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(la => la.EsCreacion);
            builder.Ignore(la => la.EsActualizacion);
            builder.Ignore(la => la.EsEliminacion);
            builder.Ignore(la => la.EsLogin);
            builder.Ignore(la => la.TuvoError);
            builder.Ignore(la => la.TieneCambios);
            builder.Ignore(la => la.DuracionSegundos);
            builder.Ignore(la => la.EsOperacionLenta);
            builder.Ignore(la => la.EsCritico);
            builder.Ignore(la => la.CantidadCamposModificados);
            builder.Ignore(la => la.Resumen);

            // Constraints
            builder.HasCheckConstraint("CK_LogsAuditoria_FechaHora",
                "`FechaHora` <= GETDATE()");

            builder.HasCheckConstraint("CK_LogsAuditoria_DuracionMs",
                "`DuracionMs` IS NULL OR `DuracionMs` >= 0");

            builder.HasCheckConstraint("CK_LogsAuditoria_CodigoResultado",
                "`CodigoResultado` IS NULL OR (`CodigoResultado` >= 100 AND `CodigoResultado` <= 599)");
        }
    }
}