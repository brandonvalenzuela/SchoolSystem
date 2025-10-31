namespace SchoolSystem.Domain.Enums.Comunicacion
{
    /// <summary>
    /// Tipos de notificación utilizados en el sistema (canal/propósito)
    /// </summary>
    public enum TipoNotificacion
    {
       /// <summary>
       /// Notificación general sin categoría específica
       /// </summary>
       General = 1,

       /// <summary>
       /// Notificación relacionada con aspectos académicos
       /// (calificaciones, tareas, avisos de clase, etc.)
       /// </summary>
       Academico = 2,

       /// <summary>
       /// Notificación disciplinaria o relacionada con conducta
       /// </summary>
       Disciplinario = 3,

       /// <summary>
       /// Notificación relacionada con pagos, facturación o finanzas
       /// </summary>
       Financiero = 4,

       /// <summary>
       /// Notificación urgente que requiere atención inmediata
       /// </summary>
       Urgente = 5,

       /// <summary>
       /// Notificación sobre eventos (actos, reuniones, actividades)
       /// </summary>
       Evento = 6,

       /// <summary>
       /// Notificación relacionada con asistencia
       /// </summary>
       Asistencia = 7,

       /// <summary>
       /// Notificación sobre tareas (entregas, fechas límite)
       /// </summary>
       Tarea = 8,

       /// <summary>
       /// Notificación sobre calificaciones individual o grupal
       /// </summary>
       Calificacion = 9,

       /// <summary>
       /// Recordatorio o aviso programado
       /// </summary>
       Recordatorio = 10
    }
}