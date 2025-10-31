using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Enums.Conducta;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Conducta
{
    /// <summary>
    /// Entidad que representa un registro de conducta de un alumno.
    /// Puede ser positivo (reconocimiento) o negativo (incidencia).
    /// </summary>
    public class RegistroConducta : BaseEntity, IAuditableEntity, ISoftDeletable
    {
        /// <summary>
        /// ID de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// ID del alumno involucrado
        /// </summary>
        [Required]
        public int AlumnoId { get; set; }

        /// <summary>
        /// ID del maestro que registra el incidente
        /// </summary>
        [Required]
        public int MaestroId { get; set; }

        /// <summary>
        /// ID del grupo donde ocurri� (opcional)
        /// </summary>
        public int? GrupoId { get; set; }

        /// <summary>
        /// Tipo de conducta registrada
        /// </summary>
        [Required]
        public TipoConducta Tipo { get; set; }

        /// <summary>
        /// Categor�a de la conducta
        /// </summary>
        [Required]
        public CategoriaConducta Categoria { get; set; }

        /// <summary>
        /// Gravedad del incidente (para conductas negativas)
        /// </summary>
        public GravedadIncidente? Gravedad { get; set; }

        /// <summary>
        /// T�tulo o resumen breve del incidente
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }

        /// <summary>
        /// Descripci�n detallada del incidente
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string Descripcion { get; set; }

        /// <summary>
        /// Fecha y hora del incidente
        /// </summary>
        [Required]
        public DateTime FechaHoraIncidente { get; set; }

        /// <summary>
        /// Lugar donde ocurri�
        /// </summary>
        [MaxLength(200)]
        public string Lugar { get; set; }

        /// <summary>
        /// Puntos asignados (positivos) o restados (negativos)
        /// </summary>
        [Required]
        public int Puntos { get; set; }

        /// <summary>
        /// Testigos del incidente (nombres o IDs)
        /// </summary>
        [MaxLength(500)]
        public string Testigos { get; set; }

        /// <summary>
        /// Evidencia adjunta (URLs de archivos)
        /// </summary>
        [MaxLength(1000)]
        public string EvidenciaUrls { get; set; }

        /// <summary>
        /// ID de la sanci�n aplicada (si aplica)
        /// </summary>
        public int? SancionId { get; set; }

        /// <summary>
        /// Acciones tomadas inmediatamente
        /// </summary>
        [MaxLength(1000)]
        public string AccionesTomadas { get; set; }

        /// <summary>
        /// Indica si los padres fueron notificados
        /// </summary>
        public bool PadresNotificados { get; set; }

        /// <summary>
        /// Fecha y hora de notificaci�n a padres
        /// </summary>
        public DateTime? FechaNotificacionPadres { get; set; }

        /// <summary>
        /// M�todo de notificaci�n usado
        /// </summary>
        [MaxLength(50)]
        public string MetodoNotificacion { get; set; }

        /// <summary>
        /// Respuesta o comentarios de los padres
        /// </summary>
        [MaxLength(1000)]
        public string RespuestaPadres { get; set; }

        /// <summary>
        /// Seguimiento requerido
        /// </summary>
        public bool RequiereSeguimiento { get; set; }

        /// <summary>
        /// Fecha para seguimiento
        /// </summary>
        public DateTime? FechaSeguimiento { get; set; }

        /// <summary>
        /// Notas de seguimiento
        /// </summary>
        [MaxLength(1000)]
        public string NotasSeguimiento { get; set; }

        /// <summary>
        /// Estado del registro
        /// </summary>
        [Required]
        public EstadoRegistroConducta Estado { get; set; }

        /// <summary>
        /// ID del periodo escolar
        /// </summary>
        public int? PeriodoId { get; set; }

        #region Propiedades de Auditor�a (IAuditableEntity)

        /// <summary>
        /// Fecha y hora de creaci�n del registro
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha y hora de la �ltima actualizaci�n
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// ID del usuario que cre� el registro
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// ID del usuario que realiz� la �ltima actualizaci�n
        /// </summary>
        public int? UpdatedBy { get; set; }

        #endregion

        #region Soft Delete (ISoftDeletable)

        /// <summary>
        /// Indica si el registro ha sido eliminado l�gicamente
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Fecha de eliminaci�n l�gica
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// ID del usuario que elimin� el registro
        /// </summary>
        public int? DeletedBy { get; set; }

        #endregion

        #region Navigation Properties (Relaciones)

        /// <summary>
        /// Alumno asociado (Navigation Property)
        /// </summary>
        public virtual Alumno Alumno { get; set; }

        /// <summary>
        /// Maestro que registr� el incidente (Navigation Property)
        /// </summary>
        public virtual Maestro Maestro { get; set; }

        /// <summary>
        /// Grupo donde ocurri� (Navigation Property)
        /// </summary>
        public virtual Grupo Grupo { get; set; }

        /// <summary>
        /// Sanci�n aplicada (Navigation Property)
        /// </summary>
        public virtual Sancion Sancion { get; set; }

        /// <summary>
        /// Escuela asociada (Navigation Property)
        /// </summary>
        public virtual Escuela Escuela { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public RegistroConducta()
        {
            Estado = EstadoRegistroConducta.Activo;
            FechaHoraIncidente = DateTime.Now;
            PadresNotificados = false;
            RequiereSeguimiento = false;
        }
        #endregion

        #region Propiedades Calculadas
        /// <summary>
        /// Indica si la conducta es de tipo positiva
        /// </summary>
        [NotMapped]
        public bool EsConductaPositiva => Tipo == TipoConducta.Positiva;

        /// <summary>
        /// Indica si la conducta es de tipo negativa
        /// </summary>
        [NotMapped]
        public bool EsConductaNegativa => Tipo == TipoConducta.Negativa;

        /// <summary>
        /// Indica si el incidente es grave
        /// </summary>
        [NotMapped]
        public bool EsGrave => Gravedad.HasValue &&
            (Gravedad == GravedadIncidente.Grave || Gravedad == GravedadIncidente.MuyGrave);

        /// <summary>
        /// Indica si requiere notificaci�n inmediata
        /// </summary>
        [NotMapped]
        public bool RequiereNotificacionInmediata => EsGrave ||
            Gravedad == GravedadIncidente.MuyGrave;

        /// <summary>
        /// D�as transcurridos desde el incidente
        /// </summary>
        [NotMapped]
        public int DiasDesdeIncidente => (DateTime.Now - FechaHoraIncidente).Days;

        /// <summary>
        /// Indica si est� pendiente de seguimiento
        /// </summary>
        [NotMapped]
        public bool EstaPendienteSeguimiento => RequiereSeguimiento &&
            FechaSeguimiento.HasValue &&
            FechaSeguimiento.Value > DateTime.Now;
        #endregion

        #region M�todos de Negocio
        /// <summary>
        /// Marca que los padres han sido notificados y registra el m�todo usado
        /// </summary>
        /// <param name="metodo">M�todo de notificaci�n (ej. "Tel�fono", "Email").</param>
        public void MarcarPadresNotificados(string metodo)
        {
            PadresNotificados = true;
            FechaNotificacionPadres = DateTime.Now;
            MetodoNotificacion = metodo;
        }

        /// <summary>
        /// Registra la respuesta de los padres
        /// </summary>
        /// <param name="respuesta">Texto con la respuesta o comentario de los padres.</param>
        public void RegistrarRespuestaPadres(string respuesta)
        {
            RespuestaPadres = respuesta;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Asigna una sanci�n y registra las acciones tomadas
        /// </summary>
        /// <param name="sancionId">ID de la sanci�n.</param>
        /// <param name="accionesTomadas">Descripci�n de las acciones tomadas.</param>
        public void AsignarSancion(int sancionId, string accionesTomadas)
        {
            SancionId = sancionId;
            AccionesTomadas = accionesTomadas;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Programa un seguimiento para una fecha futura con notas
        /// </summary>
        /// <param name="fecha">Fecha programada para seguimiento.</param>
        /// <param name="notas">Notas o instrucciones para el seguimiento.</param>
        public void ProgramarSeguimiento(DateTime fecha, string notas)
        {
            RequiereSeguimiento = true;
            FechaSeguimiento = fecha;
            NotasSeguimiento = notas;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca el seguimiento como completado y anexa notas finales
        /// </summary>
        /// <param name="notasFinales">Notas finales del seguimiento.</param>
        public void CompletarSeguimiento(string notasFinales)
        {
            RequiereSeguimiento = false;
            NotasSeguimiento += $"\n[Completado {DateTime.Now:yyyy-MM-dd}]: {notasFinales}";
            Estado = EstadoRegistroConducta.Resuelto;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Cambia el estado del registro de conducta
        /// </summary>
        /// <param name="nuevoEstado">Nuevo estado a aplicar.</param>
        public void CambiarEstado(EstadoRegistroConducta nuevoEstado)
        {
            Estado = nuevoEstado;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Calcula el impacto en puntos seg�n tipo y gravedad
        /// </summary>
        /// <returns>Valor entero con el impacto en puntos (positivo o negativo).</returns>
        public int CalcularPuntosImpacto()
        {
            // Para conductas positivas, suma puntos
            if (EsConductaPositiva)
                return Math.Abs(Puntos);

            // Para conductas negativas, resta puntos seg�n gravedad
            if (EsConductaNegativa && Gravedad.HasValue)
            {
                var multiplicador = Gravedad switch
                {
                    GravedadIncidente.Leve => 1,
                    GravedadIncidente.Moderada => 2,
                    GravedadIncidente.Grave => 3,
                    GravedadIncidente.MuyGrave => 5,
                    _ => 1
                };
                return -Math.Abs(Puntos) * multiplicador;
            }

            return -Math.Abs(Puntos);
        }

        /// <summary>
        /// Valida que el registro tenga la informaci�n m�nima requerida
        /// </summary>
        /// <returns>True si el registro es v�lido, false en caso contrario.</returns>
        public bool ValidarRegistro()
        {
            if (string.IsNullOrWhiteSpace(Titulo) || string.IsNullOrWhiteSpace(Descripcion))
                return false;

            if (AlumnoId <= 0 || MaestroId <= 0)
                return false;

            if (FechaHoraIncidente > DateTime.Now)
                return false;

            // Si es negativa, debe tener gravedad
            if (EsConductaNegativa && !Gravedad.HasValue)
                return false;

            return true;
        }
        #endregion
    }
}