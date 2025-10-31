using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Calendario;
using SchoolSystem.Domain.Enums.Calendario;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Evento
    /// </summary>
    public class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            // Nombre de tabla
            builder.ToTable("Eventos");

            // Clave primaria
            builder.HasKey(e => e.Id);

            // Propiedades requeridas
            builder.Property(e => e.EscuelaId)
                .IsRequired();

            builder.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Descripcion)
                .HasColumnType("LONGTEXT");

            builder.Property(e => e.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            // Fechas
            builder.Property(e => e.FechaInicio)
                .IsRequired();

            builder.Property(e => e.FechaFin)
                .IsRequired(false);

            builder.Property(e => e.TodoElDia)
                .IsRequired()
                .HasDefaultValue(false);

            // Destinatarios
            builder.Property(e => e.GruposAfectadosJson)
                .HasColumnName("GruposAfectados")
                .HasColumnType("LONGTEXT");

            builder.Property(e => e.AplicaATodos)
                .IsRequired()
                .HasDefaultValue(true);

            // Detalles
            builder.Property(e => e.Ubicacion)
                .HasMaxLength(200);

            builder.Property(e => e.RecordatorioMinutos)
                .IsRequired(false);

            builder.Property(e => e.Color)
                .HasMaxLength(20);

            builder.Property(e => e.Prioridad)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10)
                .HasDefaultValue(PrioridadEvento.Normal);

            // Configuración
            builder.Property(e => e.CreadoPorId)
                .IsRequired();

            builder.Property(e => e.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.RecordatoriosEnviados)
                .HasDefaultValue(false);

            builder.Property(e => e.FechaEnvioRecordatorios)
                .IsRequired(false);

            builder.Property(e => e.EsRecurrente)
                .HasDefaultValue(false);

            builder.Property(e => e.ConfiguracionRecurrencia)
                .HasColumnType("LONGTEXT");

            // Archivos
            builder.Property(e => e.ArchivoAdjuntoUrl)
                .HasMaxLength(500);

            builder.Property(e => e.ArchivoAdjuntoNombre)
                .HasMaxLength(200);

            // Auditoría
            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.CreatedBy)
                .IsRequired(false);

            builder.Property(e => e.UpdatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(e => e.CreadoPor)
                .WithMany()
                .HasForeignKey(e => e.CreadoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(e => e.EscuelaId)
                .HasDatabaseName("IX_Eventos_EscuelaId");

            builder.HasIndex(e => e.FechaInicio)
                .HasDatabaseName("IX_Eventos_FechaInicio");

            builder.HasIndex(e => e.Tipo)
                .HasDatabaseName("IX_Eventos_Tipo");

            builder.HasIndex(e => new { e.EscuelaId, e.FechaInicio, e.Activo })
                .HasDatabaseName("IX_Eventos_Escuela_Fecha_Activo");

            builder.HasIndex(e => new { e.FechaInicio, e.FechaFin })
                .HasDatabaseName("IX_Eventos_FechaInicio_FechaFin");

            builder.HasIndex(e => e.CreadoPorId)
                .HasDatabaseName("IX_Eventos_CreadoPorId");

            builder.HasIndex(e => new { e.RecordatoriosEnviados, e.FechaInicio })
                .HasDatabaseName("IX_Eventos_Recordatorios_Fecha");

            builder.HasIndex(e => e.Prioridad)
                .HasDatabaseName("IX_Eventos_Prioridad");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(e => e.YaPaso);
            builder.Ignore(e => e.EnCurso);
            builder.Ignore(e => e.EsProximo);
            builder.Ignore(e => e.EsHoy);
            builder.Ignore(e => e.DuracionHoras);
            builder.Ignore(e => e.DuracionDias);
            builder.Ignore(e => e.DiasHastaEvento);
            builder.Ignore(e => e.HorasHastaEvento);
            builder.Ignore(e => e.GruposAfectados);
            builder.Ignore(e => e.TieneArchivo);
            builder.Ignore(e => e.DebeEnviarRecordatorios);
            builder.Ignore(e => e.FechaEnvioRecordatorio);
            builder.Ignore(e => e.EsMomentoDeRecordar);

            // Constraints
            builder.HasCheckConstraint("CK_Eventos_Fechas",
                "`FechaFin` IS NULL OR `FechaFin` >= `FechaInicio`");

            builder.HasCheckConstraint("CK_Eventos_RecordatorioMinutos",
                "`RecordatorioMinutos` IS NULL OR `RecordatorioMinutos` >= 0");
        }
    }
}