using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad AlumnoPadre
    /// </summary>
    public class AlumnoPadreConfiguration : IEntityTypeConfiguration<AlumnoPadre>
    {
        /// <summary>
        /// Configura la entidad AlumnoPadre
        /// </summary>
        /// <param name="builder">Constructor de la entidad</param>
        public void Configure(EntityTypeBuilder<AlumnoPadre> builder)
        {
            #region Tabla

            builder.ToTable("AlumnoPadres");

            #endregion

            #region Clave Primaria

            builder.HasKey(ap => ap.Id);

            builder.Property(ap => ap.Id)
                .ValueGeneratedOnAdd();

            #endregion

            #region Propiedades

            // Alumno ID
            builder.Property(ap => ap.AlumnoId)
                .IsRequired();

            // Padre ID
            builder.Property(ap => ap.PadreId)
                .IsRequired();

            // Relación
            builder.Property(ap => ap.Relacion)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Es Tutor Principal
            builder.Property(ap => ap.EsTutorPrincipal)
                .HasDefaultValue(false)
                .IsRequired();

            // Autorizado Recoger
            builder.Property(ap => ap.AutorizadoRecoger)
                .HasDefaultValue(true)
                .IsRequired();

            // Recibe Notificaciones
            builder.Property(ap => ap.RecibeNotificaciones)
                .HasDefaultValue(true)
                .IsRequired();

            // Vive Con Alumno
            builder.Property(ap => ap.ViveConAlumno)
                .HasDefaultValue(true)
                .IsRequired();

            #endregion

            #region Índices

            // Índice único compuesto (un padre no puede estar relacionado dos veces con el mismo alumno)
            builder.HasIndex(ap => new { ap.AlumnoId, ap.PadreId })
                .HasDatabaseName("IX_Alumno_Padre_Unique")
                .IsUnique();

            // Índice en PadreId
            builder.HasIndex(ap => ap.PadreId)
                .HasDatabaseName("IX_Alumno_Padre_Padre_Id");

            #endregion

            #region Relaciones

            // Relación con Alumno
            builder.HasOne(ap => ap.Alumno)
                .WithMany(a => a.AlumnoPadres)
                .HasForeignKey(ap => ap.AlumnoId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Relación con Padre
            builder.HasOne(ap => ap.Padre)
                .WithMany(p => p.AlumnoPadres)
                .HasForeignKey(ap => ap.PadreId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
