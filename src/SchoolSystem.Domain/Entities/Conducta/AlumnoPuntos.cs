using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Enums.Conducta;

namespace SchoolSystem.Domain.Entities.Conducta
{
    /// <summary>
    /// Sistema de puntos y gamificación para alumnos.
    /// Maneja puntos, niveles, rankings, insignias y estadísticas.
    /// </summary>
    public class AlumnoPuntos : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Identificador único del alumno
        /// </summary>
        public int AlumnoId { get; set; }

        /// <summary>
        /// Alumno relacionado (Navigation Property)
        /// </summary>
        public virtual Alumno Alumno { get; set; }

        /// <summary>
        /// Período escolar actual
        /// </summary>
        public int? PeriodoEscolarId { get; set; }

        /// <summary>
        /// Ciclo escolar (ej: "2024-2025")
        /// </summary>
        public int CicloEscolarId { get; set; }
        public virtual CicloEscolar? Ciclo { get; set; }



        #endregion

        #region Puntos Totales y Por Categoría

        /// <summary>
        /// Puntos totales acumulados (histórico)
        /// </summary>
        [Required]
        public int PuntosTotales { get; set; }

        /// <summary>
        /// Puntos del período actual
        /// </summary>
        [Required]
        public int PuntosPeriodoActual { get; set; }

        /// <summary>
        /// Puntos académicos (tareas, exámenes, participación)
        /// </summary>
        public int PuntosAcademicos { get; set; }

        /// <summary>
        /// Puntos de conducta (comportamiento, valores)
        /// </summary>
        public int PuntosConducta { get; set; }

        /// <summary>
        /// Puntos deportivos (educación física, torneos)
        /// </summary>
        public int PuntosDeportivos { get; set; }

        /// <summary>
        /// Puntos culturales (arte, música, eventos)
        /// </summary>
        public int PuntosCulturales { get; set; }

        /// <summary>
        /// Puntos sociales (trabajo en equipo, ayuda a compañeros)
        /// </summary>
        public int PuntosSociales { get; set; }

        /// <summary>
        /// Puntos de asistencia y puntualidad
        /// </summary>
        public int PuntosAsistencia { get; set; }

        /// <summary>
        /// Puntos extra (eventos especiales, concursos)
        /// </summary>
        public int PuntosExtra { get; set; }

        #endregion

        #region Sistema de Niveles

        /// <summary>
        /// Nivel actual del alumno
        /// </summary>
        [Required]
        public int NivelActual { get; set; }

        /// <summary>
        /// Experiencia en el nivel actual
        /// </summary>
        public int ExperienciaActual { get; set; }

        /// <summary>
        /// Experiencia necesaria para el siguiente nivel
        /// </summary>
        public int ExperienciaSiguienteNivel { get; set; }

        /// <summary>
        /// Título del nivel (ej: "Principiante", "Avanzado", "Maestro")
        /// </summary>
        [StringLength(50)]
        public string TituloNivel { get; set; }

        /// <summary>
        /// Color o tema del nivel (para UI)
        /// </summary>
        [StringLength(20)]
        public string ColorNivel { get; set; }

        #endregion

        #region Rankings

        /// <summary>
        /// Posición en el ranking del grupo
        /// </summary>
        public int? RankingGrupo { get; set; }

        /// <summary>
        /// Total de alumnos en el grupo (para calcular percentil)
        /// </summary>
        public int? TotalAlumnosGrupo { get; set; }

        /// <summary>
        /// Posición en el ranking del grado
        /// </summary>
        public int? RankingGrado { get; set; }

        /// <summary>
        /// Total de alumnos en el grado
        /// </summary>
        public int? TotalAlumnosGrado { get; set; }

        /// <summary>
        /// Posición en el ranking de la escuela
        /// </summary>
        public int? RankingEscuela { get; set; }

        /// <summary>
        /// Total de alumnos en la escuela
        /// </summary>
        public int? TotalAlumnosEscuela { get; set; }

        /// <summary>
        /// Cambio de posición desde la última actualización
        /// </summary>
        public int CambioRanking { get; set; }

        #endregion

        #region Rachas y Logros

        /// <summary>
        /// Días consecutivos con asistencia perfecta
        /// </summary>
        public int RachaAsistencia { get; set; }

        /// <summary>
        /// Días consecutivos sin incidentes negativos
        /// </summary>
        public int RachaBuenaConducta { get; set; }

        /// <summary>
        /// Tareas consecutivas entregadas a tiempo
        /// </summary>
        public int RachaTareas { get; set; }

        /// <summary>
        /// Mejor racha histórica de asistencia
        /// </summary>
        public int MejorRachaAsistencia { get; set; }

        /// <summary>
        /// Mejor racha histórica de conducta
        /// </summary>
        public int MejorRachaConducta { get; set; }

        /// <summary>
        /// Mejor racha histórica de tareas
        /// </summary>
        public int MejorRachaTareas { get; set; }

        #endregion

        #region Estadísticas y Métricas

        /// <summary>
        /// Promedio de puntos por día
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal PromedioPuntosDiario { get; set; }

        /// <summary>
        /// Promedio de puntos por semana
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal PromedioPuntosSemanal { get; set; }

        /// <summary>
        /// Tendencia de puntos (positiva, negativa, estable)
        /// </summary>
        [StringLength(20)]
        public string Tendencia { get; set; }

        /// <summary>
        /// Porcentaje de mejora respecto al período anterior
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeMejora { get; set; }

        #endregion

        #region Fechas Importantes

        /// <summary>
        /// Fecha de la última actualización de puntos
        /// </summary>
        public DateTime? UltimaActualizacionPuntos { get; set; }

        /// <summary>
        /// Fecha del último cambio de nivel
        /// </summary>
        public DateTime? FechaUltimoNivel { get; set; }

        /// <summary>
        /// Fecha del último logro obtenido
        /// </summary>
        public DateTime? FechaUltimoLogro { get; set; }

        /// <summary>
        /// Fecha de reinicio de puntos del período
        /// </summary>
        public DateTime? FechaReinicioPeriodo { get; set; }

        #endregion

        #region Configuración y Preferencias

        /// <summary>
        /// Notificaciones de puntos activadas
        /// </summary>
        public bool NotificacionesActivas { get; set; }

        /// <summary>
        /// Mostrar en rankings públicos
        /// </summary>
        public bool MostrarEnRankings { get; set; }

        /// <summary>
        /// Avatar o imagen de perfil del jugador
        /// </summary>
        [StringLength(500)]
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// Lema o frase motivacional personalizada
        /// </summary>
        [StringLength(100)]
        public string? Lema { get; set; }

        #endregion

        #region Relaciones

        /// <summary>
        /// Historial de cambios de puntos (Navigation Property)
        /// </summary>
        public virtual ICollection<HistorialPuntos> HistorialPuntos { get; set; }

        /// <summary>
        /// Insignias ganadas por el alumno (Navigation Property)
        /// </summary>
        public virtual ICollection<AlumnoInsignia> InsigniasGanadas { get; set; } // Relación con AlumnoInsignia

        #endregion

        #region Auditoría (IAuditableEntity)

        /// <summary>
        /// Fecha y hora de creación del registro
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Usuario que creó el registro (cadena por compatibilidad con BaseEntity)
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>   
        /// Fecha y hora de la última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Usuario que realizó la última actualización (cadena por compatibilidad con BaseEntity)
        /// </summary>
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public AlumnoPuntos()
        {
            PuntosTotales = 0;
            PuntosPeriodoActual = 0;
            NivelActual = 1;
            ExperienciaActual = 0;
            ExperienciaSiguienteNivel = 100;
            TituloNivel = "Principiante";
            ColorNivel = "#4CAF50";
            CambioRanking = 0;
            NotificacionesActivas = true;
            MostrarEnRankings = true;
            Tendencia = "Estable";

            HistorialPuntos = new HashSet<HistorialPuntos>();
            InsigniasGanadas = new HashSet<AlumnoInsignia>();
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Porcentaje de progreso al siguiente nivel
        /// </summary>
        public decimal PorcentajeProgresoNivel
        {
            get
            {
                if (ExperienciaSiguienteNivel == 0) return 100;
                return (decimal)ExperienciaActual / ExperienciaSiguienteNivel * 100;
            }
        }

        /// <summary>
        /// Percentil en el ranking del grupo
        /// </summary>
        public decimal? PercentilGrupo
        {
            get
            {
                if (!RankingGrupo.HasValue || !TotalAlumnosGrupo.HasValue || TotalAlumnosGrupo == 0)
                    return null;
                return (decimal)(TotalAlumnosGrupo - RankingGrupo + 1) / TotalAlumnosGrupo * 100;
            }
        }

        /// <summary>
        /// Indica si está en el top10 del grupo
        /// </summary>
        public bool EsTop10Grupo => RankingGrupo.HasValue && RankingGrupo <= 10;

        /// <summary>
        /// Indica si está en el top3 del grupo
        /// </summary>
        public bool EsTop3Grupo => RankingGrupo.HasValue && RankingGrupo <= 3;

        /// <summary>
        /// Total de insignias ganadas
        /// </summary>
        public int TotalInsignias => InsigniasGanadas?.Count ?? 0;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Agrega puntos y actualiza estadísticas
        /// </summary>
        /// <param name="puntos">Cantidad de puntos a agregar (puede ser negativa)</param>
        /// <param name="categoria">Categoría de los puntos</param>
        /// <param name="descripcion">Descripción o motivo del cambio</param>
        public void AgregarPuntos(int puntos, CategoriaPuntos categoria, string descripcion)
        {
            // Actualizar puntos totales
            PuntosTotales += puntos;
            PuntosPeriodoActual += puntos;

            // Actualizar categoría específica
            switch (categoria)
            {
                case CategoriaPuntos.Academico:
                    PuntosAcademicos += puntos;
                    break;
                case CategoriaPuntos.Conducta:
                    PuntosConducta += puntos;
                    break;
                case CategoriaPuntos.Deportivo:
                    PuntosDeportivos += puntos;
                    break;
                case CategoriaPuntos.Cultural:
                    PuntosCulturales += puntos;
                    break;
                case CategoriaPuntos.Social:
                    PuntosSociales += puntos;
                    break;
                case CategoriaPuntos.Asistencia:
                    PuntosAsistencia += puntos;
                    break;
                case CategoriaPuntos.Extra:
                    PuntosExtra += puntos;
                    break;
            }

            // Actualizar experiencia
            ExperienciaActual += Math.Abs(puntos);

            // Verificar si sube de nivel
            VerificarNivel();

            // Actualizar fecha
            UltimaActualizacionPuntos = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Verifica y actualiza el nivel según la experiencia
        /// </summary>
        private void VerificarNivel()
        {
            while (ExperienciaActual >= ExperienciaSiguienteNivel)
            {
                ExperienciaActual -= ExperienciaSiguienteNivel;
                NivelActual++;

                // Calcular experiencia necesaria para siguiente nivel (progresión exponencial)
                ExperienciaSiguienteNivel = CalcularExperienciaSiguienteNivel(NivelActual);

                // Actualizar título del nivel
                ActualizarTituloNivel();

                FechaUltimoNivel = DateTime.Now;
            }
        }

        /// <summary>
        /// Calcula la experiencia necesaria para el siguiente nivel
        /// </summary>
        /// <param name="nivel">Nivel objetivo</param>
        /// <returns>Experiencia requerida</returns>
        private int CalcularExperienciaSiguienteNivel(int nivel)
        {
            // Fórmula:100 * nivel *1.5
            return (int)(100 * nivel * 1.5);
        }

        /// <summary>
        /// Actualiza el título y color según el nivel actual
        /// </summary>
        private void ActualizarTituloNivel()
        {
            TituloNivel = NivelActual switch
            {
                <= 5 => "Principiante",
                <= 10 => "Aprendiz",
                <= 15 => "Estudiante",
                <= 20 => "Avanzado",
                <= 25 => "Experto",
                <= 30 => "Maestro",
                <= 40 => "Gran Maestro",
                <= 50 => "Campeón",
                _ => "Leyenda"
            };

            ColorNivel = NivelActual switch
            {
                <= 5 => "#4CAF50",  // Verde
                <= 10 => "#2196F3", // Azul
                <= 15 => "#9C27B0", // Púrpura
                <= 20 => "#FF9800", // Naranja
                <= 25 => "#F44336", // Rojo
                <= 30 => "#FFD700", // Dorado
                <= 40 => "#E5E4E2", // Platino
                _ => "#B9F2FF"      // Diamante
            };
        }

        /// <summary>
        /// Actualiza las rachas
        /// </summary>
        /// <param name="tipo">Tipo de racha a actualizar</param>
        /// <param name="mantener">Si se mantiene la racha (true) o se reinicia (false)</param>
        public void ActualizarRacha(TipoRacha tipo, bool mantener)
        {
            switch (tipo)
            {
                case TipoRacha.Asistencia:
                    if (mantener)
                    {
                        RachaAsistencia++;
                        if (RachaAsistencia > MejorRachaAsistencia)
                            MejorRachaAsistencia = RachaAsistencia;
                    }
                    else
                    {
                        RachaAsistencia = 0;
                    }
                    break;

                case TipoRacha.Conducta:
                    if (mantener)
                    {
                        RachaBuenaConducta++;
                        if (RachaBuenaConducta > MejorRachaConducta)
                            MejorRachaConducta = RachaBuenaConducta;
                    }
                    else
                    {
                        RachaBuenaConducta = 0;
                    }
                    break;

                case TipoRacha.Tareas:
                    if (mantener)
                    {
                        RachaTareas++;
                        if (RachaTareas > MejorRachaTareas)
                            MejorRachaTareas = RachaTareas;
                    }
                    else
                    {
                        RachaTareas = 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// Actualiza el ranking y calcula el cambio
        /// </summary>
        public void ActualizarRanking(int nuevoRankingGrupo, int totalGrupo,
                                      int? nuevoRankingGrado = null, int? totalGrado = null,
                                      int? nuevoRankingEscuela = null, int? totalEscuela = null)
        {
            // Calcular cambio de posición
            if (RankingGrupo.HasValue)
            {
                CambioRanking = RankingGrupo.Value - nuevoRankingGrupo;
            }

            // Actualizar rankings
            RankingGrupo = nuevoRankingGrupo;
            TotalAlumnosGrupo = totalGrupo;

            if (nuevoRankingGrado.HasValue)
            {
                RankingGrado = nuevoRankingGrado;
                TotalAlumnosGrado = totalGrado;
            }

            if (nuevoRankingEscuela.HasValue)
            {
                RankingEscuela = nuevoRankingEscuela;
                TotalAlumnosEscuela = totalEscuela;
            }

            // Actualizar tendencia
            if (CambioRanking > 0)
                Tendencia = "Mejorando";
            else if (CambioRanking < 0)
                Tendencia = "Bajando";
            else
                Tendencia = "Estable";
        }

        /// <summary>
        /// Reinicia los puntos del período
        /// </summary>
        public void ReiniciarPeriodo()
        {
            PuntosPeriodoActual = 0;
            PuntosAcademicos = 0;
            PuntosConducta = 0;
            PuntosDeportivos = 0;
            PuntosCulturales = 0;
            PuntosSociales = 0;
            PuntosAsistencia = 0;
            PuntosExtra = 0;
            FechaReinicioPeriodo = DateTime.Now;
            CambioRanking = 0;
        }

        /// <summary>
        /// Calcula estadísticas de puntos
        /// </summary>
        /// <param name="historial">Lista con el historial de puntos</param>
        public void CalcularEstadisticas(List<HistorialPuntos> historial)
        {
            if (historial == null || !historial.Any()) return;

            var ahora = DateTime.Now;
            var hace7Dias = ahora.AddDays(-7);
            var hace30Dias = ahora.AddDays(-30);

            // Promedio diario (últimos30 días)
            var puntosMes = historial
                .Where(h => h.Fecha >= hace30Dias)
                .Sum(h => h.PuntosObtenidos);
            PromedioPuntosDiario = puntosMes / 30m;

            // Promedio semanal (últimas4 semanas)
            PromedioPuntosSemanal = puntosMes / 4m;

            // Calcular tendencia comparando última semana vs semana anterior
            var puntosUltimaSemana = historial
                .Where(h => h.Fecha >= hace7Dias)
                .Sum(h => h.PuntosObtenidos);

            var puntosSemanaAnterior = historial
                .Where(h => h.Fecha >= hace7Dias.AddDays(-7) && h.Fecha < hace7Dias)
                .Sum(h => h.PuntosObtenidos);

            if (puntosSemanaAnterior > 0)
            {
                PorcentajeMejora = ((puntosUltimaSemana - puntosSemanaAnterior) / (decimal)puntosSemanaAnterior) * 100;

                if (PorcentajeMejora > 10)
                    Tendencia = "Mejorando";
                else if (PorcentajeMejora < -10)
                    Tendencia = "Bajando";
                else
                    Tendencia = "Estable";
            }
        }
        #endregion
    }
}