using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Auditoria;
using SchoolSystem.Domain.Enums.Auditoria;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Sincronizacion
    /// </summary>
    public class SincronizacionConfiguration : IEntityTypeConfiguration<Sincronizacion>
    {
        public void Configure(EntityTypeBuilder<Sincronizacion> builder)
        {
            // Nombre de tabla
            builder.ToTable("Sincronizaciones");

            // Clave primaria
            builder.HasKey(s => s.Id);

            // Propiedades requeridas
            builder.Property(s => s.EscuelaId)
                .IsRequired(false);

            builder.Property(s => s.UsuarioId)
                .IsRequired(false);

            builder.Property(s => s.NombreUsuario)
                .HasMaxLength(200);

            // Dispositivo/Cliente
            builder.Property(s => s.DispositivoId)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.NombreDispositivo)
                .HasMaxLength(200);

            builder.Property(s => s.TipoDispositivo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.SistemaOperativo)
                .HasMaxLength(100);

            builder.Property(s => s.VersionCliente)
                .HasMaxLength(50);

            // Tipo y estado
            builder.Property(s => s.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(TipoSincronizacion.Manual);

            builder.Property(s => s.Direccion)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(DireccionSincronizacion.Bidireccional);

            builder.Property(s => s.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(EstadoSincronizacion.Pendiente);

            // Fechas
            builder.Property(s => s.FechaInicio)
                .IsRequired();

            builder.Property(s => s.FechaFin)
                .IsRequired(false);

            builder.Property(s => s.DuracionMs)
                .IsRequired(false);

            builder.Property(s => s.UltimaSincronizacionExitosa)
                .IsRequired(false);

            // Entidades sincronizadas
            builder.Property(s => s.EntidadesSincronizadas)
                .HasColumnType("LONGTEXT");

            builder.Property(s => s.TotalEntidades)
                .HasDefaultValue(0);

            // Contadores
            builder.Property(s => s.RegistrosCreados)
                .HasDefaultValue(0);

            builder.Property(s => s.RegistrosActualizados)
                .HasDefaultValue(0);

            builder.Property(s => s.RegistrosEliminados)
                .HasDefaultValue(0);

            builder.Property(s => s.RegistrosSinCambios)
                .HasDefaultValue(0);

            builder.Property(s => s.CantidadErrores)
                .HasDefaultValue(0);

            builder.Property(s => s.TamanioDatos)
                .IsRequired(false);

            // Errores
            builder.Property(s => s.TuvoErrores)
                .HasDefaultValue(false);

            builder.Property(s => s.MensajeError)
                .HasColumnType("LONGTEXT");

            builder.Property(s => s.DetalleErrores)
                .HasColumnType("LONGTEXT");

            builder.Property(s => s.StackTrace)
                .HasColumnType("LONGTEXT");

            // Verificación
            builder.Property(s => s.HashVerificacion)
                .HasMaxLength(500);

            builder.Property(s => s.HashCliente)
                .HasMaxLength(500);

            builder.Property(s => s.VerificacionExitosa)
                .IsRequired(false);

            // Conflictos
            builder.Property(s => s.CantidadConflictos)
                .HasDefaultValue(0);

            builder.Property(s => s.DetalleConflictos)
                .HasColumnType("LONGTEXT");

            builder.Property(s => s.EstrategiaResolucion)
                .HasMaxLength(50);

            // Metadata
            builder.Property(s => s.DireccionIP)
                .HasMaxLength(45);

            builder.Property(s => s.DatosAdicionales)
                .HasColumnType("LONGTEXT");

            builder.Property(s => s.Observaciones)
                .HasColumnType("LONGTEXT");

            // Configuración
            builder.Property(s => s.ModoSincronizacion)
                .HasMaxLength(50)
                .HasDefaultValue("Incremental");

            builder.Property(s => s.Prioridad)
                .HasDefaultValue(5);

            builder.Property(s => s.Reintentos)
                .HasDefaultValue(0);

            // Relaciones
            builder.HasOne(s => s.Usuario)
                .WithMany()
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(s => s.EscuelaId)
                .HasDatabaseName("IX_Sincronizaciones_EscuelaId");

            builder.HasIndex(s => s.UsuarioId)
                .HasDatabaseName("IX_Sincronizaciones_UsuarioId");

            builder.HasIndex(s => s.DispositivoId)
                .HasDatabaseName("IX_Sincronizaciones_DispositivoId");

            builder.HasIndex(s => s.TipoDispositivo)
                .HasDatabaseName("IX_Sincronizaciones_TipoDispositivo");

            builder.HasIndex(s => s.Tipo)
                .HasDatabaseName("IX_Sincronizaciones_Tipo");

            builder.HasIndex(s => s.Estado)
                .HasDatabaseName("IX_Sincronizaciones_Estado");

            builder.HasIndex(s => s.FechaInicio)
                .HasDatabaseName("IX_Sincronizaciones_FechaInicio");

            builder.HasIndex(s => s.FechaFin)
                .HasDatabaseName("IX_Sincronizaciones_FechaFin");

            builder.HasIndex(s => s.TuvoErrores)
                .HasDatabaseName("IX_Sincronizaciones_TuvoErrores");

            builder.HasIndex(s => new { s.DispositivoId, s.FechaInicio })
                .HasDatabaseName("IX_Sincronizaciones_Dispositivo_Fecha");

            builder.HasIndex(s => new { s.EscuelaId, s.Estado, s.FechaInicio })
                .HasDatabaseName("IX_Sincronizaciones_Escuela_Estado_Fecha");

            builder.HasIndex(s => new { s.Estado, s.FechaInicio })
                .HasDatabaseName("IX_Sincronizaciones_Estado_Fecha");

            builder.HasIndex(s => new { s.DispositivoId, s.Estado })
                .HasDatabaseName("IX_Sincronizaciones_Dispositivo_Estado");

            builder.HasIndex(s => s.UltimaSincronizacionExitosa)
                .HasDatabaseName("IX_Sincronizaciones_UltimaSincronizacionExitosa");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(s => s.EstaPendiente);
            builder.Ignore(s => s.EstaEnProceso);
            builder.Ignore(s => s.EstaCompletada);
            builder.Ignore(s => s.Fallo);
            builder.Ignore(s => s.EstaCancelada);
            builder.Ignore(s => s.TotalRegistrosAfectados);
            builder.Ignore(s => s.TotalRegistrosProcesados);
            builder.Ignore(s => s.PorcentajeExito);
            builder.Ignore(s => s.DuracionSegundos);
            builder.Ignore(s => s.DuracionCalculada);
            builder.Ignore(s => s.TamanioDatosKB);
            builder.Ignore(s => s.TamanioDatosMB);
            builder.Ignore(s => s.VelocidadRegistrosPorSegundo);
            builder.Ignore(s => s.EsAutomatica);
            builder.Ignore(s => s.EsManual);
            builder.Ignore(s => s.TuvoConflictos);
            builder.Ignore(s => s.VerificacionFallo);
            builder.Ignore(s => s.EsSincronizacionCompleta);
            builder.Ignore(s => s.EsSincronizacionIncremental);
            builder.Ignore(s => s.Resumen);

            // Constraints
            builder.HasCheckConstraint("CK_Sincronizaciones_FechaFin",
                "`FechaFin` IS NULL OR `FechaFin` >= `FechaInicio`");

            builder.HasCheckConstraint("CK_Sincronizaciones_DuracionMs",
                "`DuracionMs` IS NULL OR `DuracionMs` >= 0");

            builder.HasCheckConstraint("CK_Sincronizaciones_Contadores",
                "`RegistrosCreados` >= 0 AND `RegistrosActualizados` >= 0 AND `RegistrosEliminados` >= 0 AND `RegistrosSinCambios` >= 0");

            builder.HasCheckConstraint("CK_Sincronizaciones_Errores",
                "`CantidadErrores` >= 0 AND `CantidadConflictos` >= 0");

            builder.HasCheckConstraint("CK_Sincronizaciones_TamanioDatos",
                "`TamanioDatos` IS NULL OR `TamanioDatos` >= 0");

            builder.HasCheckConstraint("CK_Sincronizaciones_Prioridad",
                "`Prioridad` >= 1 AND `Prioridad` <= 10");

            builder.HasCheckConstraint("CK_Sincronizaciones_Reintentos",
                "`Reintentos` >= 0");

            builder.HasCheckConstraint("CK_Sincronizaciones_TotalEntidades",
                "`TotalEntidades` >= 0");
        }
    }
}