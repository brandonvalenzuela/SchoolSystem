namespace SchoolSystem.Web.Enum
{
    public enum EstadoAsistencia
    {
        /// <summary>
        /// Alumno presente
        /// </summary>
        Presente = 1,

        /// <summary>
        /// Alumno ausente sin justificación
        /// </summary>
        Falta = 2,

        /// <summary>
        /// Alumno llegó tarde
        /// </summary>
        Retardo = 3,

        /// <summary>
        /// Falta justificada
        /// </summary>
        Justificada = 4,

        /// <summary>
        /// Permiso autorizado
        /// </summary>
        Permiso = 5
    }
}
