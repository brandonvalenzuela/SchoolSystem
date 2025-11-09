using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Enums;
using SchoolSystem.Domain.Enums.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad Alumno
    /// </summary>
    public class AlumnoConfiguration : IEntityTypeConfiguration<Alumno>
    {
        /// <summary>
        /// Configura la entidad Alumno para EF Core
        /// </summary>
        /// <param name="builder">Constructor de la entidad</param>
        public void Configure(EntityTypeBuilder<Alumno> builder)
        {
            #region Tabla

            builder.ToTable("Alumnos");

            #endregion

            #region Clave Primaria

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            #endregion

            #region Foreign Keys

            // Escuela ID (Multi-tenant)
            builder.Property(a => a.EscuelaId)
                .IsRequired();

            // Usuario ID (opcional)
            builder.Property(a => a.UsuarioId);

            #endregion

            #region Datos de Identificación

            // Matrícula
            builder.Property(a => a.Matricula)
                .HasMaxLength(50)
                .IsRequired();

            // CURP
            builder.Property(a => a.CURP)
                .HasMaxLength(18);

            #endregion

            #region Datos Personales

            // Nombre
            builder.Property(a => a.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            // Apellido Paterno
            builder.Property(a => a.ApellidoPaterno)
                .HasMaxLength(100)
                .IsRequired();

            // Apellido Materno
            builder.Property(a => a.ApellidoMaterno)
                .HasMaxLength(100);

            // Fecha Nacimiento
            builder.Property(a => a.FechaNacimiento)
                .HasColumnType("DATE")
                .IsRequired();

            // Género
            builder.Property(a => a.Genero)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Foto URL
            builder.Property(a => a.FotoUrl)
                .IsRequired(false)
                .HasMaxLength(500);

            #endregion

            #region Datos de Contacto

            // Dirección
            builder.Property(a => a.Direccion)
                .HasMaxLength(300);

            // Teléfono
            builder.Property(a => a.Telefono)
                .HasMaxLength(20);

            // Email
            builder.Property(a => a.Email)
                .HasMaxLength(100);

            #endregion

            #region Información Médica

            // Tipo Sangre
            builder.Property(a => a.TipoSangre)
                .HasMaxLength(5);

            // Alergias
            builder.Property(a => a.Alergias)
                .IsRequired(false)
                .HasColumnType("TEXT");

            // Condiciones Médicas
            builder.Property(a => a.CondicionesMedicas)
                .IsRequired(false)
                .HasColumnType("TEXT");

            // Medicamentos
            builder.Property(a => a.Medicamentos)
                .IsRequired(false)
                .HasColumnType("TEXT");

            #endregion

            #region Contacto de Emergencia

            // Contacto Emergencia Nombre
            builder.Property(a => a.ContactoEmergenciaNombre)
                .HasMaxLength(100);

            // Contacto Emergencia Teléfono
            builder.Property(a => a.ContactoEmergenciaTelefono)
                .HasMaxLength(20);

            // Contacto Emergencia Relación
            builder.Property(a => a.ContactoEmergenciaRelacion)
                .HasMaxLength(50);

            #endregion

            #region Control Académico

            // Fecha Ingreso
            builder.Property(a => a.FechaIngreso)
                .HasColumnType("DATE")
                .IsRequired();

            // Fecha Baja
            builder.Property(a => a.FechaBaja)
                .HasColumnType("DATE");

            // Motivo Baja
            builder.Property(a => a.MotivoBaja)
                .IsRequired(false)
                .HasColumnType("TEXT");

            // Estatus
            builder.Property(a => a.Estatus)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(EstatusAlumno.Activo)
                .IsRequired();

            // Observaciones
            builder.Property(a => a.Observaciones)
                .IsRequired(false)
                .HasColumnType("TEXT");

            #endregion

            #region Auditoría

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .IsRequired();

            builder.Property(a => a.CreatedBy);

            builder.Property(a => a.UpdatedBy);

            #endregion

            #region Soft Delete

            // Is Deleted
            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            // Deleted At
            builder.Property(a => a.DeletedAt)
                .HasColumnType("DATETIME");

            // Deleted By
            builder.Property(a => a.DeletedBy);

            // Query Filter para Soft Delete (solo mostrar no eliminados por defecto)
            builder.HasQueryFilter(a => !a.IsDeleted);

            #endregion

            #region Propiedades Calculadas (Ignorar)

            // NombreCompleto es calculado
            builder.Ignore(a => a.NombreCompleto);

            // Edad es calculado
            builder.Ignore(a => a.Edad);

            // Antigüedad es calculado
            builder.Ignore(a => a.AntiguedadEnAños);

            #endregion

            #region Índices

            // Índice único en Matrícula
            builder.HasIndex(a => a.Matricula)
                .HasDatabaseName("IX_Alumno_Matricula")
                .IsUnique();

            // Índice único en CURP (si existe)
            builder.HasIndex(a => a.CURP)
                .HasDatabaseName("IX_Alumno_Curp")
                .IsUnique()
                .HasFilter("curp IS NOT NULL");

            // Índice en Estatus
            builder.HasIndex(a => a.Estatus)
                .HasDatabaseName("IX_Alumno_Estatus");

            // Índice compuesto en EscuelaId y Estatus (búsquedas muy frecuentes)
            builder.HasIndex(a => new { a.EscuelaId, a.Estatus })
                .HasDatabaseName("IX_Alumno_Escuela_Estatus");

            // Índice en IsDeleted para Soft Delete
            builder.HasIndex(a => a.IsDeleted)
                .HasDatabaseName("IX_Alumno_Is_Deleted");

            #endregion

            #region Relaciones

            // Relación con Escuela
            builder.HasOne(a => a.Escuela)
                .WithMany(e => e.Alumnos)
                .HasForeignKey(a => a.EscuelaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación opcional con Usuario
            builder.HasOne(a => a.Usuario)
                .WithOne(u => u.Alumno)
                .HasForeignKey<Alumno>(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Inscripciones (un alumno tiene muchas inscripciones)
            builder.HasMany(a => a.Inscripciones)
                .WithOne(i => i.Alumno)
                .HasForeignKey(i => i.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con AlumnoPadres (muchos a muchos con Padres)
            builder.HasMany(a => a.AlumnoPadres)
                .WithOne(ap => ap.Alumno)
                .HasForeignKey(ap => ap.AlumnoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Asistencias
            builder.HasMany(a => a.Asistencias)
                .WithOne(ast => ast.Alumno)
                .HasForeignKey(ast => ast.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Calificaciones
            builder.HasMany(a => a.Calificaciones)
                .WithOne(c => c.Alumno)
                .HasForeignKey(c => c.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }

  
}