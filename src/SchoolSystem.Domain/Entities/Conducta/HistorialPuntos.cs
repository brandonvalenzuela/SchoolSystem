using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Conducta;
using SchoolSystem.Domain.Enums.Conducta;
using static SchoolSystem.Domain.Entities.Conducta.AlumnoPuntos;

/// <summary>
/// Registro histórico de cambios en puntos
/// </summary>
public class HistorialPuntos : BaseEntity
{
    /// <summary>
    /// ID del registro de puntos asociado
    /// </summary>
    public int AlumnoPuntosId { get; set; }

    /// <summary>
    /// Referencia a AlumnoPuntos (Navigation Property)
    /// </summary>
    public virtual AlumnoPuntos AlumnoPuntos { get; set; }

    /// <summary>
    /// Fecha del cambio
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Puntos obtenidos en este registro
    /// </summary>
    public int PuntosObtenidos { get; set; }

    /// <summary>
    /// Categoría de los puntos
    /// </summary>
    public CategoriaPuntos Categoria { get; set; }

    /// <summary>
    /// Descripción o motivo del cambio
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Origen del cambio (ID de la entidad que generó los puntos)
    /// </summary>
    public string OrigenId { get; set; }

    /// <summary>
    /// Tipo de origen ("Calificacion", "Conducta", "Asistencia", etc.)
    /// </summary>
    public string TipoOrigen { get; set; }

    /// <summary>
    /// Puntos acumulados después de este cambio
    /// </summary>
    public int PuntosAcumulados { get; set; }
}