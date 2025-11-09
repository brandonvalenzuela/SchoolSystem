using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Maestro
    /// </summary>
    public class MaestroConfiguration : IEntityTypeConfiguration<Maestro>
    {
        public void Configure(EntityTypeBuilder<Maestro> builder)
        {
            // Nombre de tabla
            builder.ToTable("Maestros");

            // Clave primaria
            builder.HasKey(m => m.Id);

            // Propiedades requeridas
            builder.Property(m => m.EscuelaId)
                .IsRequired();

            builder.Property(m => m.UsuarioId)
                .IsRequired();

            builder.Property(m => m.Estatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(SchoolSystem.Domain.Enums.Academico.EstatusLaboral.Activo);

            // Información laboral
            builder.Property(m => m.NumeroEmpleado)
                .HasMaxLength(50);

            builder.Property(m => m.FechaIngreso)
                .IsRequired(false);

            builder.Property(m => m.FechaBaja)
                .IsRequired(false);

            builder.Property(m => m.TipoContrato)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(m => m.Salario)
                .IsRequired(false)
                .HasColumnType("decimal(18,2)");

            // Información profesional
            builder.Property(m => m.CedulaProfesional)
                .HasMaxLength(50);

            builder.Property(m => m.Especialidad)
                .HasMaxLength(200);

            builder.Property(m => m.TituloAcademico)
                .HasMaxLength(100);

            builder.Property(m => m.Universidad)
                .HasMaxLength(200);

            builder.Property(m => m.AñoGraduacion)
                .IsRequired(false);

            builder.Property(m => m.AñosExperiencia)
                .IsRequired(false);

            // Certificaciones
            builder.Property(m => m.Certificaciones)
                .IsRequired(false)
                .HasColumnType("LONGTEXT");

            builder.Property(m => m.Capacitaciones)
                .IsRequired(false)
                .HasColumnType("LONGTEXT");

            builder.Property(m => m.Idiomas)
                .IsRequired(false)
                .HasMaxLength(500);

            // Información adicional
            builder.Property(m => m.Observaciones)
                .IsRequired(false)
                .HasColumnType("LONGTEXT");

            builder.Property(m => m.HorarioAtencion)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(m => m.DisponibleExtracurriculares)
                .HasDefaultValue(false);

            // Relaciones
            builder.HasOne(m => m.Escuela)
                .WithMany()
                .HasForeignKey(m => m.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Usuario)
                .WithMany()
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.GruposTitular)
                .WithOne(g => g.MaestroTitular)
                .HasForeignKey(g => g.MaestroTitularId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(m => m.AsignacionesDeGrupoMateria)
                .WithOne(am => am.Maestro)
                .HasForeignKey(am => am.MaestroId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(m => m.EscuelaId)
                .HasDatabaseName("IX_Maestros_EscuelaId");

            builder.HasIndex(m => m.UsuarioId)
                .HasDatabaseName("IX_Maestros_UsuarioId");

            builder.HasIndex(m => m.NumeroEmpleado)
                .HasDatabaseName("IX_Maestros_NumeroEmpleado");

            builder.HasIndex(m => m.Estatus)
                .HasDatabaseName("IX_Maestros_Estatus");

            builder.HasIndex(m => m.CedulaProfesional)
                .HasDatabaseName("IX_Maestros_CedulaProfesional");

            builder.HasIndex(m => m.Especialidad)
                .HasDatabaseName("IX_Maestros_Especialidad");

            // Índice único: Un usuario solo puede tener un registro de maestro por escuela
            builder.HasIndex(m => new { m.EscuelaId, m.UsuarioId })
                .IsUnique()
                .HasDatabaseName("IX_Maestros_Escuela_Usuario_Unique");

            // Índice único: Número de empleado único por escuela
            builder.HasIndex(m => new { m.EscuelaId, m.NumeroEmpleado })
                .IsUnique()
                .HasDatabaseName("IX_Maestros_Escuela_NumeroEmpleado_Unique");

            builder.HasIndex(m => new { m.EscuelaId, m.Estatus })
                .HasDatabaseName("IX_Maestros_Escuela_Estatus");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(m => m.CantidadGruposTitular);
            builder.Ignore(m => m.CantidadMaterias);
            builder.Ignore(m => m.AntiguedadEnAños);

            // Constraints
            builder.HasCheckConstraint("CK_Maestros_Salario",
                "`Salario` IS NULL OR `Salario` >= 0");

            builder.HasCheckConstraint("CK_Maestros_AñoGraduacion",
                "`AñoGraduacion` IS NULL OR (`AñoGraduacion` >= 1900 AND `AñoGraduacion` <= 2100)"); // <-- CORREGIDO

            builder.HasCheckConstraint("CK_Maestros_AñosExperiencia",
                "`AñosExperiencia` IS NULL OR `AñosExperiencia` >= 0");

            builder.HasCheckConstraint("CK_Maestros_Fechas",
                "`FechaBaja` IS NULL OR `FechaIngreso` IS NULL OR (`FechaBaja` >= `FechaIngreso`)"); // <-- CORREGIDO
        }
    }
}