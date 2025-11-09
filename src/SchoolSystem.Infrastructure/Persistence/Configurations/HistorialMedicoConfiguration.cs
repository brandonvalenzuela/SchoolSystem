using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Medico;
using SchoolSystem.Domain.Enums.Medico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad HistorialMedico
    /// </summary>
    public class HistorialMedicoConfiguration : IEntityTypeConfiguration<HistorialMedico>
    {
        public void Configure(EntityTypeBuilder<HistorialMedico> builder)
        {
            // Nombre de tabla
            builder.ToTable("HistorialMedico");

            // Clave primaria
            builder.HasKey(hm => hm.Id);

            // Propiedades requeridas
            builder.Property(hm => hm.EscuelaId)
                .IsRequired();

            builder.Property(hm => hm.ExpedienteMedicoId)
                .IsRequired();

            builder.Property(hm => hm.TipoIncidente)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(hm => hm.FechaIncidente)
                .IsRequired();

            // Descripción del incidente
            builder.Property(hm => hm.Descripcion)
                .IsRequired(false)
                .HasColumnType("LONGTEXT");

            builder.Property(hm => hm.Sintomas)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(hm => hm.LugarIncidente)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(hm => hm.OcurrioEnEscuela)
                .HasDefaultValue(false);

            // Diagnóstico
            builder.Property(hm => hm.Diagnostico)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(hm => hm.Gravedad)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Atención médica
            builder.Property(hm => hm.TratamientoAplicado)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(hm => hm.MedicoAtencion)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(hm => hm.EspecialidadMedico)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(hm => hm.LugarAtencion)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(hm => hm.InstitucionAtencion)
                .HasMaxLength(200)
                .IsRequired(false);

            // Hospitalización
            builder.Property(hm => hm.RequirioHospitalizacion)
                .HasDefaultValue(false);

            builder.Property(hm => hm.FechaIngresoHospital)
                .IsRequired(false);

            builder.Property(hm => hm.FechaAltaHospital)
                .IsRequired(false);

            builder.Property(hm => hm.DiasHospitalizado)
                .IsRequired(false);

            builder.Property(hm => hm.MotivoHospitalizacion)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Medicamentos y procedimientos
            builder.Property(hm => hm.MedicamentosRecetados)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(hm => hm.ProcedimientosRealizados)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(hm => hm.RequirioCirugia)
                .HasDefaultValue(false);

            builder.Property(hm => hm.DescripcionCirugia)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            // Seguimiento
            builder.Property(hm => hm.RequiereSeguimiento)
                .HasDefaultValue(false);

            builder.Property(hm => hm.FechaProximaConsulta)
                .IsRequired(false);

            builder.Property(hm => hm.IndicacionesSeguimiento)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(hm => hm.CasoCerrado)
                .HasDefaultValue(false);

            builder.Property(hm => hm.FechaCierreCaso)
                .IsRequired(false);

            // Notificación
            builder.Property(hm => hm.PadresNotificados)
                .HasDefaultValue(false);

            builder.Property(hm => hm.FechaNotificacionPadres)
                .IsRequired(false);

            builder.Property(hm => hm.PersonaQueNotifico)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(hm => hm.MedioNotificacion)
                .HasMaxLength(50)
                .IsRequired(false);

            // Documentación
            builder.Property(hm => hm.DocumentosUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(hm => hm.Observaciones)
                .HasColumnType("LONGTEXT")
                .IsRequired(false);

            builder.Property(hm => hm.RegistradoPorId)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(hm => hm.ExpedienteMedico)
                .WithMany(em => em.HistorialMedico)
                .HasForeignKey(hm => hm.ExpedienteMedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(hm => hm.RegistradoPor)
                .WithMany()
                .HasForeignKey(hm => hm.RegistradoPorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(hm => hm.EscuelaId)
                .HasDatabaseName("IX_HistorialMedico_EscuelaId");

            builder.HasIndex(hm => hm.ExpedienteMedicoId)
                .HasDatabaseName("IX_HistorialMedico_ExpedienteMedicoId");

            builder.HasIndex(hm => hm.TipoIncidente)
                .HasDatabaseName("IX_HistorialMedico_TipoIncidente");

            builder.HasIndex(hm => hm.FechaIncidente)
                .HasDatabaseName("IX_HistorialMedico_FechaIncidente");

            builder.HasIndex(hm => hm.Gravedad)
                .HasDatabaseName("IX_HistorialMedico_Gravedad");

            builder.HasIndex(hm => hm.OcurrioEnEscuela)
                .HasDatabaseName("IX_HistorialMedico_OcurrioEnEscuela");

            builder.HasIndex(hm => hm.RequirioHospitalizacion)
                .HasDatabaseName("IX_HistorialMedico_RequirioHospitalizacion");

            builder.HasIndex(hm => hm.CasoCerrado)
                .HasDatabaseName("IX_HistorialMedico_CasoCerrado");

            builder.HasIndex(hm => hm.RequiereSeguimiento)
                .HasDatabaseName("IX_HistorialMedico_RequiereSeguimiento");

            builder.HasIndex(hm => hm.FechaProximaConsulta)
                .HasDatabaseName("IX_HistorialMedico_FechaProximaConsulta");

            builder.HasIndex(hm => new { hm.ExpedienteMedicoId, hm.FechaIncidente })
                .HasDatabaseName("IX_HistorialMedico_Expediente_Fecha");

            builder.HasIndex(hm => new { hm.TipoIncidente, hm.Gravedad, hm.FechaIncidente })
                .HasDatabaseName("IX_HistorialMedico_Tipo_Gravedad_Fecha");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(hm => hm.EsGrave);
            builder.Ignore(hm => hm.EsAccidente);
            builder.Ignore(hm => hm.EsEnfermedad);
            builder.Ignore(hm => hm.DiasDesdeIncidente);
            builder.Ignore(hm => hm.EstaHospitalizado);
            builder.Ignore(hm => hm.DiasHospitalizacionCalculados);
            builder.Ignore(hm => hm.TieneDocumentos);
            builder.Ignore(hm => hm.TieneSeguimientoPendiente);
            builder.Ignore(hm => hm.DiasHastaProximaConsulta);
            builder.Ignore(hm => hm.ProximaConsultaCercana);
            builder.Ignore(hm => hm.ConsultaVencida);
            builder.Ignore(hm => hm.ResumenBreve);

            // Constraints
            builder.HasCheckConstraint("CK_HistorialMedico_FechaAltaHospital",
                "`FechaAltaHospital` IS NULL OR `FechaAltaHospital` >= `FechaIngresoHospital`");

            builder.HasCheckConstraint("CK_HistorialMedico_DiasHospitalizado",
                "`DiasHospitalizado` IS NULL OR `DiasHospitalizado` >= 0");

            builder.HasCheckConstraint("CK_HistorialMedico_Hospitalizacion",
                "(`RequirioHospitalizacion` = 0) OR (`RequirioHospitalizacion` = 1 AND `FechaIngresoHospital` IS NOT NULL)");
        }
    }
}