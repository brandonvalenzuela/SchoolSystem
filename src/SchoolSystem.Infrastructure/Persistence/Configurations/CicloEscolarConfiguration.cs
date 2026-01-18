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
    public class CicloEscolarConfiguration : IEntityTypeConfiguration<CicloEscolar>
    {
        public void Configure(EntityTypeBuilder<CicloEscolar> builder)
        {
            builder.ToTable("CiclosEscolares");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.EscuelaId)
                .IsRequired();

            builder.Property(x => x.Clave)
                .HasColumnType("varchar(20)")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Nombre)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.FechaInicio)
                .HasColumnType("date")
                .IsRequired(false);

            builder.Property(x => x.FechaFin)
                .HasColumnType("date")
                .IsRequired(false);

            builder.Property(x => x.EsActual)
                .HasColumnType("tinyint(1)")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime(6)")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("datetime(6)")
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired(false);

            builder.Property(x => x.UpdatedBy)
                .IsRequired(false);

            // UNIQUE KEY IX_CiclosEscolares_Escuela_Clave_Unique (EscuelaId, Clave)
            builder.HasIndex(x => new { x.EscuelaId, x.Clave })
                .IsUnique()
                .HasDatabaseName("IX_CiclosEscolares_Escuela_Clave_Unique");

            // KEY IX_CiclosEscolares_Escuela_Actual (EscuelaId, EsActual)
            builder.HasIndex(x => new { x.EscuelaId, x.EsActual })
                .HasDatabaseName("IX_CiclosEscolares_Escuela_Actual");

            // FK con ON DELETE RESTRICT
            builder.HasOne(x => x.Escuela)
                .WithMany()
                .HasForeignKey(x => x.EscuelaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
