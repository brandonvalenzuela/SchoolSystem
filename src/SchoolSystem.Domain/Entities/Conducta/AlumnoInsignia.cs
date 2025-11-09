using SchoolSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Entities.Conducta
{
    /// <summary>
    /// Relación entre alumno y las insignias ganadas
    /// </summary>
    public class AlumnoInsignia : BaseEntity
    {
        /// <summary>
        /// ID del registro de puntos asociado (NO DEBE SER ANULABLE)
        /// </summary>
        public int AlumnoPuntosId { get; set; }

        /// <summary>
        /// Referencia a AlumnoPuntos (NO DEBE SER ANULABLE)
        /// </summary>
        public virtual AlumnoPuntos AlumnoPuntos { get; set; }

        /// <summary>
        /// ID de la insignia (NO DEBE SER ANULABLE)
        /// </summary>
        public int InsigniaId { get; set; }

        /// <summary>
        /// Insignia (NO DEBE SER ANULABLE)
        /// </summary>
        public virtual Insignia Insignia { get; set; }

        /// <summary>
        /// Fecha en que se obtuvo la insignia
        /// </summary>
        public DateTime FechaObtencion { get; set; }

        /// <summary>
        /// Motivo por el cual se otorgó
        /// </summary>
        public string? Motivo { get; set; }

        /// <summary>
        /// Indica si la insignia está marcada como favorita
        /// </summary>
        public bool EsFavorita { get; set; }

        /// <summary>
        /// Veces que se obtuvo la insignia
        /// </summary>
        public int VecesObtenida { get; set; }
    }

}
