using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystem.Domain.Entities.Configuracion;
using SchoolSystem.Domain.Enums.Configuracion;

namespace SchoolSystem.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework para la entidad PreferenciaUsuario
    /// </summary>
    public class PreferenciaUsuarioConfiguration : IEntityTypeConfiguration<PreferenciaUsuario>
    {
        public void Configure(EntityTypeBuilder<PreferenciaUsuario> builder)
        {
            // Nombre de tabla
            builder.ToTable("PreferenciasUsuario");

            // Clave primaria
            builder.HasKey(pu => pu.Id);

            // Propiedades requeridas
            builder.Property(pu => pu.EscuelaId)
                .IsRequired(false);

            builder.Property(pu => pu.UsuarioId)
                .IsRequired();

            builder.Property(pu => pu.Clave)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pu => pu.Valor)
                .HasColumnType("LONGTEXT");

            builder.Property(pu => pu.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30)
                .HasDefaultValue(TipoPreferencia.Personalizacion);

            // Descripción
            builder.Property(pu => pu.Nombre)
                .HasMaxLength(300);

            builder.Property(pu => pu.Descripcion)
                .HasMaxLength(500);

            builder.Property(pu => pu.Categoria)
                .IsRequired()
                .HasMaxLength(100);

            // Configuración
            builder.Property(pu => pu.Grupo)
                .HasMaxLength(100);

            builder.Property(pu => pu.TipoDato)
                .HasMaxLength(50)
                .HasDefaultValue("String");

            builder.Property(pu => pu.ValorPredeterminado)
                .HasColumnType("LONGTEXT");

            // Sincronización
            builder.Property(pu => pu.EsSincronizable)
                .HasDefaultValue(true);

            builder.Property(pu => pu.FechaUltimaSincronizacion)
                .IsRequired(false);

            builder.Property(pu => pu.DispositivoOrigen)
                .HasMaxLength(200);

            builder.Property(pu => pu.HashSincronizacion)
                .HasMaxLength(500);

            // Alcance
            builder.Property(pu => pu.Alcance)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(AlcancePreferencia.Usuario);

            builder.Property(pu => pu.EsPreferenciaSistema)
                .HasDefaultValue(false);

            builder.Property(pu => pu.EsPrivada)
                .HasDefaultValue(false);

            // Estado
            builder.Property(pu => pu.Activa)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(pu => pu.FechaUltimoCambio)
                .IsRequired(false);

            builder.Property(pu => pu.ValorAnterior)
                .HasColumnType("LONGTEXT");

            // Validación
            builder.Property(pu => pu.RequiereValidacion)
                .HasDefaultValue(false);

            builder.Property(pu => pu.ExpresionValidacion)
                .HasMaxLength(500);

            builder.Property(pu => pu.ValoresPermitidos)
                .HasColumnType("LONGTEXT");

            // Metadata
            builder.Property(pu => pu.Etiquetas)
                .HasMaxLength(500);

            builder.Property(pu => pu.Orden)
                .HasDefaultValue(0);

            builder.Property(pu => pu.Icono)
                .HasMaxLength(50);

            builder.Property(pu => pu.Observaciones)
                .HasColumnType("LONGTEXT");

            // Auditoría
            builder.Property(pu => pu.CreatedAt)
                .IsRequired();

            builder.Property(pu => pu.CreatedBy)
                .IsRequired(false);

            builder.Property(pu => pu.UpdatedAt)
                .IsRequired();

            builder.Property(pu => pu.UpdatedBy)
                .IsRequired(false);

            // Relaciones
            builder.HasOne(pu => pu.Usuario)
                .WithMany()
                .HasForeignKey(pu => pu.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(pu => pu.EscuelaId)
                .HasDatabaseName("IX_PreferenciasUsuario_EscuelaId");

            builder.HasIndex(pu => pu.UsuarioId)
                .HasDatabaseName("IX_PreferenciasUsuario_UsuarioId");

            // Índice único compuesto: Usuario + Clave
            builder.HasIndex(pu => new { pu.UsuarioId, pu.Clave })
                .IsUnique()
                .HasDatabaseName("IX_PreferenciasUsuario_Usuario_Clave_Unique");

            builder.HasIndex(pu => pu.Tipo)
                .HasDatabaseName("IX_PreferenciasUsuario_Tipo");

            builder.HasIndex(pu => pu.Categoria)
                .HasDatabaseName("IX_PreferenciasUsuario_Categoria");

            builder.HasIndex(pu => pu.Activa)
                .HasDatabaseName("IX_PreferenciasUsuario_Activa");

            builder.HasIndex(pu => pu.EsSincronizable)
                .HasDatabaseName("IX_PreferenciasUsuario_EsSincronizable");

            builder.HasIndex(pu => pu.Alcance)
                .HasDatabaseName("IX_PreferenciasUsuario_Alcance");

            builder.HasIndex(pu => new { pu.UsuarioId, pu.Tipo, pu.Activa })
                .HasDatabaseName("IX_PreferenciasUsuario_Usuario_Tipo_Activa");

            builder.HasIndex(pu => new { pu.UsuarioId, pu.Categoria, pu.Activa })
                .HasDatabaseName("IX_PreferenciasUsuario_Usuario_Categoria_Activa");

            builder.HasIndex(pu => new { pu.Categoria, pu.Grupo, pu.Orden })
                .HasDatabaseName("IX_PreferenciasUsuario_Categoria_Grupo_Orden");

            builder.HasIndex(pu => pu.FechaUltimaSincronizacion)
                .HasDatabaseName("IX_PreferenciasUsuario_FechaUltimaSincronizacion");

            builder.HasIndex(pu => pu.FechaUltimoCambio)
                .HasDatabaseName("IX_PreferenciasUsuario_FechaUltimoCambio");

            // Propiedades calculadas (ignoradas en BD)
            builder.Ignore(pu => pu.TieneValor);
            builder.Ignore(pu => pu.ValorEfectivo);
            builder.Ignore(pu => pu.EsTema);
            builder.Ignore(pu => pu.EsNotificacion);
            builder.Ignore(pu => pu.EsPrivacidad);
            builder.Ignore(pu => pu.PuedeSincronizarse);
            builder.Ignore(pu => pu.FueSincronizada);
            builder.Ignore(pu => pu.DiasSinSincronizar);
            builder.Ignore(pu => pu.PuedeSerModificada);
            builder.Ignore(pu => pu.DiasSinCambio);
            builder.Ignore(pu => pu.ModificadaRecientemente);
            builder.Ignore(pu => pu.ClaveCompleta);
            builder.Ignore(pu => pu.EsAlcanceGlobal);
            builder.Ignore(pu => pu.EsAlcanceEscuela);
            builder.Ignore(pu => pu.EsAlcanceUsuario);

            // Constraints
            builder.HasCheckConstraint("CK_PreferenciasUsuario_UsuarioId",
                "`UsuarioId` > 0");

            builder.HasCheckConstraint("CK_PreferenciasUsuario_Orden",
                "`Orden` >= 0");

            builder.HasCheckConstraint("CK_PreferenciasUsuario_AlcanceEscuela",
                "(`Alcance` != 'Escuela') OR (`Alcance` = 'Escuela' AND `EscuelaId` IS NOT NULL)");

            builder.HasCheckConstraint("CK_PreferenciasUsuario_AlcanceGlobal",
                "(`Alcance` != 'Global') OR (`Alcance` = 'Global' AND `EscuelaId` IS NULL)");
        }
    }
}