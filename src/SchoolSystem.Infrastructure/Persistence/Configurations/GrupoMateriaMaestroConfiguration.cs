using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Academico;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad GrupoMateriaMaestro
    /// </summary>
    public class GrupoMateriaMaestroConfiguration : IEntityTypeConfiguration<GrupoMateriaMaestro>
    {
        public void Configure(EntityTypeBuilder<GrupoMateriaMaestro> builder)
        {
            // Nombre de tabla
            builder.ToTable("GrupoMateriaMaestros");

            // Clave primaria
            builder.HasKey(gmm => gmm.Id);

            // Propiedades requeridas
            builder.Property(gmm => gmm.EscuelaId)
                .IsRequired();

            builder.Property(gmm => gmm.GrupoId)
                .IsRequired();

            builder.Property(gmm => gmm.MateriaId)
                .IsRequired();

            builder.Property(gmm => gmm.MaestroId)
                .IsRequired();

            builder.Property(gmm => gmm.CicloEscolar)
                .HasMaxLength(50);

            builder.Property(x => x.CicloEscolarId)
                .IsRequired(false);

            builder.HasOne(x => x.Ciclo)
                .WithMany()
                .HasForeignKey(x => x.CicloEscolarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(gmm => gmm.Horario)
                .HasMaxLength(500)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(gmm => gmm.Grupo)
                .WithMany(g => g.GrupoMateriaMaestros)
                .HasForeignKey(gmm => gmm.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gmm => gmm.Materia)
                .WithMany(m => m.AsignacionesGrupos)
                .HasForeignKey(gmm => gmm.MateriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gmm => gmm.Maestro)
                .WithMany(m => m.AsignacionesDeGrupoMateria)
                .HasForeignKey(gmm => gmm.MaestroId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(gmm => gmm.EscuelaId)
                .HasDatabaseName("IX_GrupoMateriaMaestros_EscuelaId");

            builder.HasIndex(gmm => gmm.GrupoId)
                .HasDatabaseName("IX_GrupoMateriaMaestros_GrupoId");

            builder.HasIndex(gmm => gmm.MateriaId)
                .HasDatabaseName("IX_GrupoMateriaMaestros_MateriaId");

            builder.HasIndex(gmm => gmm.MaestroId)
                .HasDatabaseName("IX_GrupoMateriaMaestros_MaestroId");

            builder.HasIndex(gmm => gmm.CicloEscolar)
                .HasDatabaseName("IX_GrupoMateriaMaestros_CicloEscolar");

            builder.HasIndex(x => new { x.EscuelaId, x.CicloEscolarId })
                .HasDatabaseName("IX_GrupoMateriaMaestros_Escuela_CicloEscolarId");

            // Índice único compuesto: Un grupo no puede tener la misma materia asignada dos veces en el mismo ciclo
            builder.HasIndex(gmm => new { gmm.EscuelaId, gmm.GrupoId, gmm.MateriaId, gmm.CicloEscolar })
                .IsUnique()
                .HasDatabaseName("IX_GrupoMateriaMaestros_Escuela_Grupo_Materia_Ciclo_Unique");

            builder.HasIndex(gmm => new { gmm.MaestroId, gmm.CicloEscolar })
                .HasDatabaseName("IX_GrupoMateriaMaestros_Maestro_Ciclo");

            builder.HasIndex(gmm => new { gmm.GrupoId, gmm.CicloEscolar })
                .HasDatabaseName("IX_GrupoMateriaMaestros_Grupo_Ciclo");
        }
    }
}