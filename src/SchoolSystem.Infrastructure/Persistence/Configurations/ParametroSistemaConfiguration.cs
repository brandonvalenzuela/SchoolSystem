using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Configuracion;
using SchoolSystem.Domain.Enums.Configuracion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad ParametroSistema
    /// </summary>
    public class ParametroSistemaConfiguration : IEntityTypeConfiguration<ParametroSistema>
    {
        public void Configure(EntityTypeBuilder<ParametroSistema> builder)
        {
            // Nombre de tabla
            builder.ToTable("ParametrosSistema");

            // Clave primaria
            builder.HasKey(ps => ps.Id);

            // Propiedades requeridas
            builder.Property(ps => ps.EscuelaId)
                .IsRequired(false);

            builder.Property(ps => ps.Clave)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(ps => ps.Valor)
                .HasColumnType("LONGTEXT");

            builder.Property(ps => ps.TipoDato)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(TipoParametro.String);

            // Descripción
            builder.Property(ps => ps.Nombre)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(ps => ps.Descripcion)
                .HasColumnType("LONGTEXT");

            builder.Property(ps => ps.Categoria)
                .IsRequired()
                .HasMaxLength(100);

            // Configuración
            builder.Property(ps => ps.ValorPredeterminado)
                .HasColumnType("LONGTEXT");

            builder.Property(ps => ps.ValoresPermitidos)
                .HasColumnType("LONGTEXT");

            builder.Property(ps => ps.ValorMinimo)
                .IsRequired(false)
                .HasColumnType("decimal(18,4)");

            builder.Property(ps => ps.ValorMaximo)
                .IsRequired(false)
                .HasColumnType("decimal(18,4)");

            builder.Property(ps => ps.ExpresionValidacion)
                .HasMaxLength(500);

            // Comportamiento
            builder.Property(ps => ps.EsGlobal)
                .HasDefaultValue(false);

            builder.Property(ps => ps.EsConfigurable)
                .HasDefaultValue(true);

            builder.Property(ps => ps.EsVisible)
                .HasDefaultValue(true);

            builder.Property(ps => ps.EsSoloLectura)
                .HasDefaultValue(false);

            builder.Property(ps => ps.RequiereReinicio)
                .HasDefaultValue(false);

            builder.Property(ps => ps.EsSensible)
                .HasDefaultValue(false);

            builder.Property(ps => ps.EstaEncriptado)
                .HasDefaultValue(false);

            // Metadata
            builder.Property(ps => ps.Grupo)
                .HasMaxLength(100);

            builder.Property(ps => ps.Orden)
                .HasDefaultValue(0);

            builder.Property(ps => ps.Etiquetas)
                .HasMaxLength(500);

            builder.Property(ps => ps.Unidad)
                .HasMaxLength(50);

            builder.Property(ps => ps.UrlAyuda)
                .HasMaxLength(500);

            // Estado
            builder.Property(ps => ps.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(ps => ps.FechaUltimoCambio)
                .IsRequired(false);

            builder.Property(ps => ps.UsuarioUltimoCambioId)
                .IsRequired(false);

            builder.Property(ps => ps.ValorAnterior)
                .HasColumnType("LONGTEXT");

            // Cache
            builder.Property(ps => ps.HabilitarCache)
                .HasDefaultValue(true);

            builder.Property(ps => ps.TiempoCacheMinutos)
                .IsRequired(false)
                .HasDefaultValue(60);

            // Observaciones
            builder.Property(ps => ps.Observaciones)
                .HasColumnType("LONGTEXT");

            // Auditoría
            builder.Property(ps => ps.CreatedAt)
                .IsRequired();

            builder.Property(ps => ps.CreatedBy)
                .IsRequired(false);

            builder.Property(ps => ps.UpdatedAt)
                .IsRequired();

            builder.Property(ps => ps.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(ps => ps.UsuarioUltimoCambio)
                .WithMany()
                .HasForeignKey(ps => ps.UsuarioUltimoCambioId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(ps => ps.EscuelaId)
                .HasDatabaseName("IX_ParametrosSistema_EscuelaId");

            // Índice único compuesto: Clave + EscuelaId
            builder.HasIndex(ps => new { ps.Clave, ps.EscuelaId })
                .IsUnique()
                .HasDatabaseName("IX_ParametrosSistema_Clave_EscuelaId_Unique");

            builder.HasIndex(ps => ps.Categoria)
                .HasDatabaseName("IX_ParametrosSistema_Categoria");

            builder.Property(ps => ps.Grupo)
                .HasMaxLength(100);

            builder.HasIndex(ps => ps.Activo)
                .HasDatabaseName("IX_ParametrosSistema_Activo");

            builder.HasIndex(ps => ps.EsGlobal)
                .HasDatabaseName("IX_ParametrosSistema_EsGlobal");

            builder.HasIndex(ps => ps.EsConfigurable)
                .HasDatabaseName("IX_ParametrosSistema_EsConfigurable");

            builder.HasIndex(ps => ps.TipoDato)
                .HasDatabaseName("IX_ParametrosSistema_TipoDato");

            builder.HasIndex(ps => new { ps.Categoria, ps.Grupo, ps.Orden })
                .HasDatabaseName("IX_ParametrosSistema_Categoria_Grupo_Orden");

            builder.HasIndex(ps => new { ps.EscuelaId, ps.Categoria, ps.Activo })
                .HasDatabaseName("IX_ParametrosSistema_Escuela_Categoria_Activo");

            builder.HasIndex(ps => ps.FechaUltimoCambio)
                .HasDatabaseName("IX_ParametrosSistema_FechaUltimoCambio");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(ps => ps.TieneValor);
            builder.Ignore(ps => ps.UsaValorPredeterminado);
            builder.Ignore(ps => ps.ValorEfectivo);
            builder.Ignore(ps => ps.EsString);
            builder.Ignore(ps => ps.EsEntero);
            builder.Ignore(ps => ps.EsDecimal);
            builder.Ignore(ps => ps.EsBooleano);
            builder.Ignore(ps => ps.EsJSON);
            builder.Ignore(ps => ps.EsFecha);
            builder.Ignore(ps => ps.PuedeSerModificado);
            builder.Ignore(ps => ps.TieneRestricciones);
            builder.Ignore(ps => ps.DiasSinCambio);
            builder.Ignore(ps => ps.ModificadoRecientemente);
            builder.Ignore(ps => ps.ClaveCompleta);

            // Constraints
            builder.HasCheckConstraint("CK_ParametrosSistema_Orden",
                "`Orden` >= 0");

            builder.HasCheckConstraint("CK_ParametrosSistema_ValorMinMax",
                "`ValorMinimo` IS NULL OR `ValorMaximo` IS NULL OR `ValorMinimo` <= `ValorMaximo`");

            builder.HasCheckConstraint("CK_ParametrosSistema_TiempoCache",
                "`TiempoCacheMinutos` IS NULL OR `TiempoCacheMinutos` > 0");

            builder.HasCheckConstraint("CK_ParametrosSistema_EsGlobal",
                "(`EsGlobal` = 0 AND `EscuelaId` IS NOT NULL) OR (`EsGlobal` = 1 AND `EscuelaId` IS NULL)");
        }
    }
}