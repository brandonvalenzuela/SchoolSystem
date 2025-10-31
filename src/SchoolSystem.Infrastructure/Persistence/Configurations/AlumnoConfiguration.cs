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

            builder.ToTable("alumnos");

            #endregion

            #region Clave Primaria

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            #endregion

            #region Foreign Keys

            // Escuela ID (Multi-tenant)
            builder.Property(a => a.EscuelaId)
                .HasColumnName("escuela_id")
                .IsRequired();

            // Usuario ID (opcional)
            builder.Property(a => a.UsuarioId)
                .HasColumnName("usuario_id");

            #endregion

            #region Datos de Identificación

            // Matrícula
            builder.Property(a => a.Matricula)
                .HasColumnName("matricula")
                .HasMaxLength(50)
                .IsRequired();

            // CURP
            builder.Property(a => a.CURP)
                .HasColumnName("curp")
                .HasMaxLength(18);

            #endregion

            #region Datos Personales

            // Nombre
            builder.Property(a => a.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsRequired();

            // Apellido Paterno
            builder.Property(a => a.ApellidoPaterno)
                .HasColumnName("apellido_paterno")
                .HasMaxLength(100)
                .IsRequired();

            // Apellido Materno
            builder.Property(a => a.ApellidoMaterno)
                .HasColumnName("apellido_materno")
                .HasMaxLength(100);

            // Fecha Nacimiento
            builder.Property(a => a.FechaNacimiento)
                .HasColumnName("fecha_nacimiento")
                .HasColumnType("DATE")
                .IsRequired();

            // Género
            builder.Property(a => a.Genero)
                .HasColumnName("genero")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Foto URL
            builder.Property(a => a.FotoUrl)
                .HasColumnName("foto_url")
                .HasMaxLength(500);

            #endregion

            #region Datos de Contacto

            // Dirección
            builder.Property(a => a.Direccion)
                .HasColumnName("direccion")
                .HasMaxLength(300);

            // Teléfono
            builder.Property(a => a.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(20);

            // Email
            builder.Property(a => a.Email)
                .HasColumnName("email")
                .HasMaxLength(100);

            #endregion

            #region Información Médica

            // Tipo Sangre
            builder.Property(a => a.TipoSangre)
                .HasColumnName("tipo_sangre")
                .HasMaxLength(5);

            // Alergias
            builder.Property(a => a.Alergias)
                .HasColumnName("alergias")
                .HasColumnType("TEXT");

            // Condiciones Médicas
            builder.Property(a => a.CondicionesMedicas)
                .HasColumnName("condiciones_medicas")
                .HasColumnType("TEXT");

            // Medicamentos
            builder.Property(a => a.Medicamentos)
                .HasColumnName("medicamentos")
                .HasColumnType("TEXT");

            #endregion

            #region Contacto de Emergencia

            // Contacto Emergencia Nombre
            builder.Property(a => a.ContactoEmergenciaNombre)
                .HasColumnName("contacto_emergencia_nombre")
                .HasMaxLength(100);

            // Contacto Emergencia Teléfono
            builder.Property(a => a.ContactoEmergenciaTelefono)
                .HasColumnName("contacto_emergencia_telefono")
                .HasMaxLength(20);

            // Contacto Emergencia Relación
            builder.Property(a => a.ContactoEmergenciaRelacion)
                .HasColumnName("contacto_emergencia_relacion")
                .HasMaxLength(50);

            #endregion

            #region Control Académico

            // Fecha Ingreso
            builder.Property(a => a.FechaIngreso)
                .HasColumnName("fecha_ingreso")
                .HasColumnType("DATE")
                .IsRequired();

            // Fecha Baja
            builder.Property(a => a.FechaBaja)
                .HasColumnName("fecha_baja")
                .HasColumnType("DATE");

            // Motivo Baja
            builder.Property(a => a.MotivoBaja)
                .HasColumnName("motivo_baja")
                .HasColumnType("TEXT");

            // Estatus
            builder.Property(a => a.Estatus)
                .HasColumnName("estatus")
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(EstatusAlumno.Activo)
                .IsRequired();

            // Observaciones
            builder.Property(a => a.Observaciones)
                .HasColumnName("observaciones")
                .HasColumnType("TEXT");

            #endregion

            #region Auditoría

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(a => a.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(a => a.UpdatedBy)
                .HasColumnName("updated_by");

            #endregion

            #region Soft Delete

            // Is Deleted
            builder.Property(a => a.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .IsRequired();

            // Deleted At
            builder.Property(a => a.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("DATETIME");

            // Deleted By
            builder.Property(a => a.DeletedBy)
                .HasColumnName("deleted_by");

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
                .HasDatabaseName("idx_alumno_matricula")
                .IsUnique();

            // Índice único en CURP (si existe)
            builder.HasIndex(a => a.CURP)
                .HasDatabaseName("idx_alumno_curp")
                .IsUnique()
                .HasFilter("curp IS NOT NULL");

            // Índice en Estatus
            builder.HasIndex(a => a.Estatus)
                .HasDatabaseName("idx_alumno_estatus");

            // Índice compuesto en EscuelaId y Estatus (búsquedas muy frecuentes)
            builder.HasIndex(a => new { a.EscuelaId, a.Estatus })
                .HasDatabaseName("idx_alumno_escuela_estatus");

            // Índice en IsDeleted para Soft Delete
            builder.HasIndex(a => a.IsDeleted)
                .HasDatabaseName("idx_alumno_is_deleted");

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