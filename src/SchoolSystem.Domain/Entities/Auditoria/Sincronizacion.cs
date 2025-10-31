using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Auditoria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Auditoria
{
    /// <summary>
    /// Control de sincronización entre dispositivos y APIs
    /// </summary>
    [Table("Sincronizaciones")]
    public class Sincronizacion : BaseEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        public int? EscuelaId { get; set; }

        /// <summary>
        /// Usuario que inició la sincronización
        /// </summary>
        public int? UsuarioId { get; set; }

        /// <summary>
        /// Nombre del usuario
        /// </summary>
        [StringLength(200)]
        public string NombreUsuario { get; set; }

        #endregion

        #region Dispositivo/Cliente

        /// <summary>
        /// Identificador único del dispositivo
        /// </summary>
        [Required]
        [StringLength(200)]
        public string DispositivoId { get; set; }

        /// <summary>
        /// Nombre del dispositivo
        /// </summary>
        [StringLength(200)]
        public string NombreDispositivo { get; set; }

        /// <summary>
        /// Tipo de dispositivo (móvil, desktop, web, API)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TipoDispositivo { get; set; }

        /// <summary>
        /// Sistema operativo
        /// </summary>
        [StringLength(100)]
        public string SistemaOperativo { get; set; }

        /// <summary>
        /// Versión del cliente/app
        /// </summary>
        [StringLength(50)]
        public string VersionCliente { get; set; }

        #endregion

        #region Tipo y Estado

        /// <summary>
        /// Tipo de sincronización
        /// </summary>
        [Required]
        public TipoSincronizacion Tipo { get; set; }

        /// <summary>
        /// Dirección de la sincronización
        /// </summary>
        [Required]
        public DireccionSincronizacion Direccion { get; set; }

        /// <summary>
        /// Estado de la sincronización
        /// </summary>
        [Required]
        public EstadoSincronizacion Estado { get; set; }

        #endregion

        #region Fechas

        /// <summary>
        /// Fecha y hora de inicio
        /// </summary>
        [Required]
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha y hora de fin
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Duración en milisegundos
        /// </summary>
        public int? DuracionMs { get; set; }

        /// <summary>
        /// Última sincronización exitosa de este dispositivo
        /// </summary>
        public DateTime? UltimaSincronizacionExitosa { get; set; }

        #endregion

        #region Entidades Sincronizadas

        /// <summary>
        /// Entidades incluidas en la sincronización (JSON array)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string EntidadesSincronizadas { get; set; }

        /// <summary>
        /// Total de entidades sincronizadas
        /// </summary>
        public int TotalEntidades { get; set; }

        #endregion

        #region Contadores

        /// <summary>
        /// Cantidad de registros creados
        /// </summary>
        public int RegistrosCreados { get; set; }

        /// <summary>
        /// Cantidad de registros actualizados
        /// </summary>
        public int RegistrosActualizados { get; set; }

        /// <summary>
        /// Cantidad de registros eliminados
        /// </summary>
        public int RegistrosEliminados { get; set; }

        /// <summary>
        /// Cantidad de registros sin cambios
        /// </summary>
        public int RegistrosSinCambios { get; set; }

        /// <summary>
        /// Cantidad de errores
        /// </summary>
        public int CantidadErrores { get; set; }

        /// <summary>
        /// Tamaño total de datos sincronizados en bytes
        /// </summary>
        public long? TamanioDatos { get; set; }

        #endregion

        #region Errores

        /// <summary>
        /// Indica si hubo errores
        /// </summary>
        public bool TuvoErrores { get; set; }

        /// <summary>
        /// Mensaje de error general
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string MensajeError { get; set; }

        /// <summary>
        /// Detalle de errores en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DetalleErrores { get; set; }

        /// <summary>
        /// Stack trace del error principal
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string StackTrace { get; set; }

        #endregion

        #region Verificación

        /// <summary>
        /// Hash de verificación de los datos enviados
        /// </summary>
        [StringLength(500)]
        public string HashVerificacion { get; set; }

        /// <summary>
        /// Hash recibido del cliente (para comparación)
        /// </summary>
        [StringLength(500)]
        public string HashCliente { get; set; }

        /// <summary>
        /// Indica si la verificación fue exitosa
        /// </summary>
        public bool? VerificacionExitosa { get; set; }

        #endregion

        #region Conflictos

        /// <summary>
        /// Cantidad de conflictos detectados
        /// </summary>
        public int CantidadConflictos { get; set; }

        /// <summary>
        /// Detalle de conflictos en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DetalleConflictos { get; set; }

        /// <summary>
        /// Estrategia de resolución de conflictos
        /// </summary>
        [StringLength(50)]
        public string EstrategiaResolucion { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Dirección IP del cliente
        /// </summary>
        [StringLength(45)]
        public string DireccionIP { get; set; }

        /// <summary>
        /// Información adicional en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DatosAdicionales { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Configuración

        /// <summary>
        /// Modo de sincronización (completa, incremental)
        /// </summary>
        [StringLength(50)]
        public string ModoSincronizacion { get; set; }

        /// <summary>
        /// Prioridad de la sincronización
        /// </summary>
        public int Prioridad { get; set; }

        /// <summary>
        /// Número de reintentos realizados
        /// </summary>
        public int Reintentos { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Usuario relacionado
        /// </summary>
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        #endregion

        #region Constructor

        public Sincronizacion()
        {
            FechaInicio = DateTime.Now;
            Estado = EstadoSincronizacion.Pendiente;
            Tipo = TipoSincronizacion.Manual;
            Direccion = DireccionSincronizacion.Bidireccional;
            TotalEntidades = 0;
            RegistrosCreados = 0;
            RegistrosActualizados = 0;
            RegistrosEliminados = 0;
            RegistrosSinCambios = 0;
            CantidadErrores = 0;
            CantidadConflictos = 0;
            TuvoErrores = false;
            ModoSincronizacion = "Incremental";
            Prioridad = 5;
            Reintentos = 0;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si está pendiente
        /// </summary>
        public bool EstaPendiente => Estado == EstadoSincronizacion.Pendiente;

        /// <summary>
        /// Indica si está en proceso
        /// </summary>
        public bool EstaEnProceso => Estado == EstadoSincronizacion.EnProceso;

        /// <summary>
        /// Indica si está completada
        /// </summary>
        public bool EstaCompletada => Estado == EstadoSincronizacion.Completada;

        /// <summary>
        /// Indica si falló
        /// </summary>
        public bool Fallo => Estado == EstadoSincronizacion.Error;

        /// <summary>
        /// Indica si fue cancelada
        /// </summary>
        public bool EstaCancelada => Estado == EstadoSincronizacion.Cancelada;

        /// <summary>
        /// Total de registros afectados
        /// </summary>
        public int TotalRegistrosAfectados => RegistrosCreados + RegistrosActualizados + RegistrosEliminados;

        /// <summary>
        /// Total de registros procesados
        /// </summary>
        public int TotalRegistrosProcesados => TotalRegistrosAfectados + RegistrosSinCambios;

        /// <summary>
        /// Porcentaje de éxito
        /// </summary>
        public decimal? PorcentajeExito
        {
            get
            {
                var total = TotalRegistrosProcesados + CantidadErrores;
                if (total == 0) return null;
                return ((decimal)TotalRegistrosProcesados / total) * 100;
            }
        }

        /// <summary>
        /// Duración en segundos
        /// </summary>
        public decimal? DuracionSegundos
        {
            get
            {
                if (!DuracionMs.HasValue) return null;
                return (decimal)DuracionMs.Value / 1000;
            }
        }

        /// <summary>
        /// Duración calculada si no está guardada
        /// </summary>
        public TimeSpan? DuracionCalculada
        {
            get
            {
                if (!FechaFin.HasValue) return null;
                return FechaFin.Value - FechaInicio;
            }
        }

        /// <summary>
        /// Tamaño de datos en KB
        /// </summary>
        public decimal? TamanioDatosKB
        {
            get
            {
                if (!TamanioDatos.HasValue) return null;
                return Math.Round((decimal)TamanioDatos.Value / 1024, 2);
            }
        }

        /// <summary>
        /// Tamaño de datos en MB
        /// </summary>
        public decimal? TamanioDatosMB
        {
            get
            {
                if (!TamanioDatos.HasValue) return null;
                return Math.Round((decimal)TamanioDatos.Value / (1024 * 1024), 2);
            }
        }

        /// <summary>
        /// Velocidad de sincronización (registros por segundo)
        /// </summary>
        public decimal? VelocidadRegistrosPorSegundo
        {
            get
            {
                if (!DuracionMs.HasValue || DuracionMs.Value == 0) return null;
                return (decimal)TotalRegistrosProcesados / ((decimal)DuracionMs.Value / 1000);
            }
        }

        /// <summary>
        /// Indica si es sincronización automática
        /// </summary>
        public bool EsAutomatica => Tipo == TipoSincronizacion.Automatica;

        /// <summary>
        /// Indica si es sincronización manual
        /// </summary>
        public bool EsManual => Tipo == TipoSincronizacion.Manual;

        /// <summary>
        /// Indica si tuvo conflictos
        /// </summary>
        public bool TuvoConflictos => CantidadConflictos > 0;

        /// <summary>
        /// Indica si la verificación falló
        /// </summary>
        public bool VerificacionFallo => VerificacionExitosa.HasValue && !VerificacionExitosa.Value;

        /// <summary>
        /// Indica si es sincronización completa
        /// </summary>
        public bool EsSincronizacionCompleta => ModoSincronizacion == "Completa";

        /// <summary>
        /// Indica si es sincronización incremental
        /// </summary>
        public bool EsSincronizacionIncremental => ModoSincronizacion == "Incremental";

        /// <summary>
        /// Resumen de la sincronización
        /// </summary>
        public string Resumen
        {
            get
            {
                var resumen = $"{Tipo} - {Estado}";
                if (EstaCompletada)
                    resumen += $" ({TotalRegistrosAfectados} registros afectados)";
                if (TuvoErrores)
                    resumen += $" ({CantidadErrores} errores)";
                return resumen;
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Inicia la sincronización
        /// </summary>
        public void Iniciar()
        {
            if (Estado != EstadoSincronizacion.Pendiente)
                throw new InvalidOperationException("Solo se pueden iniciar sincronizaciones pendientes");

            Estado = EstadoSincronizacion.EnProceso;
            FechaInicio = DateTime.Now;
        }

        /// <summary>
        /// Completa la sincronización exitosamente
        /// </summary>
        public void Completar()
        {
            if (Estado != EstadoSincronizacion.EnProceso)
                throw new InvalidOperationException("Solo se pueden completar sincronizaciones en proceso");

            Estado = EstadoSincronizacion.Completada;
            FechaFin = DateTime.Now;

            if (!DuracionMs.HasValue)
                DuracionMs = (int)(FechaFin.Value - FechaInicio).TotalMilliseconds;

            UltimaSincronizacionExitosa = FechaFin;
        }

        /// <summary>
        /// Marca la sincronización como fallida
        /// </summary>
        public void MarcarComoFallida(string mensajeError, string stackTrace = null)
        {
            Estado = EstadoSincronizacion.Error;
            TuvoErrores = true;
            MensajeError = mensajeError;
            StackTrace = stackTrace;
            FechaFin = DateTime.Now;

            if (!DuracionMs.HasValue)
                DuracionMs = (int)(FechaFin.Value - FechaInicio).TotalMilliseconds;
        }

        /// <summary>
        /// Cancela la sincronización
        /// </summary>
        public void Cancelar(string motivo = null)
        {
            if (Estado == EstadoSincronizacion.Completada)
                throw new InvalidOperationException("No se puede cancelar una sincronización completada");

            Estado = EstadoSincronizacion.Cancelada;
            FechaFin = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(motivo))
                Observaciones = motivo;

            if (!DuracionMs.HasValue)
                DuracionMs = (int)(FechaFin.Value - FechaInicio).TotalMilliseconds;
        }

        /// <summary>
        /// Registra registros creados
        /// </summary>
        public void RegistrarCreados(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa");
            RegistrosCreados += cantidad;
        }

        /// <summary>
        /// Registra registros actualizados
        /// </summary>
        public void RegistrarActualizados(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa");
            RegistrosActualizados += cantidad;
        }

        /// <summary>
        /// Registra registros eliminados
        /// </summary>
        public void RegistrarEliminados(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa");
            RegistrosEliminados += cantidad;
        }

        /// <summary>
        /// Registra registros sin cambios
        /// </summary>
        public void RegistrarSinCambios(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa");
            RegistrosSinCambios += cantidad;
        }

        /// <summary>
        /// Registra un error
        /// </summary>
        public void RegistrarError(string detalleError)
        {
            CantidadErrores++;
            TuvoErrores = true;

            if (string.IsNullOrWhiteSpace(DetalleErrores))
                DetalleErrores = $"[{DateTime.Now:HH:mm:ss}] {detalleError}";
            else
                DetalleErrores += $"\n[{DateTime.Now:HH:mm:ss}] {detalleError}";
        }

        /// <summary>
        /// Registra un conflicto
        /// </summary>
        public void RegistrarConflicto(string detalleConflicto)
        {
            CantidadConflictos++;

            if (string.IsNullOrWhiteSpace(DetalleConflictos))
                DetalleConflictos = detalleConflicto;
            else
                DetalleConflictos += $"\n{detalleConflicto}";
        }

        /// <summary>
        /// Establece el hash de verificación
        /// </summary>
        public void EstablecerHashVerificacion(string hashServidor, string hashCliente = null)
        {
            HashVerificacion = hashServidor;
            HashCliente = hashCliente;

            if (!string.IsNullOrEmpty(hashCliente))
                VerificacionExitosa = hashServidor == hashCliente;
        }

        /// <summary>
        /// Registra un reintento
        /// </summary>
        public void RegistrarReintento()
        {
            Reintentos++;
            Estado = EstadoSincronizacion.Pendiente;
        }

        /// <summary>
        /// Establece el tamaño de datos sincronizados
        /// </summary>
        public void EstablecerTamanioDatos(long bytes)
        {
            if (bytes < 0)
                throw new ArgumentException("El tamaño no puede ser negativo");
            TamanioDatos = bytes;
        }

        /// <summary>
        /// Obtiene estadísticas de la sincronización
        /// </summary>
        public string ObtenerEstadisticas()
        {
            var stats = "=== ESTADÍSTICAS DE SINCRONIZACIÓN ===\n\n";
            stats += $"Estado: {Estado}\n";
            stats += $"Tipo: {Tipo} - {Direccion}\n";
            stats += $"Duración: {DuracionSegundos:N2} segundos\n\n";
            stats += $"Registros Creados: {RegistrosCreados}\n";
            stats += $"Registros Actualizados: {RegistrosActualizados}\n";
            stats += $"Registros Eliminados: {RegistrosEliminados}\n";
            stats += $"Registros Sin Cambios: {RegistrosSinCambios}\n";
            stats += $"Total Procesados: {TotalRegistrosProcesados}\n\n";

            if (TuvoErrores)
                stats += $"⚠️ Errores: {CantidadErrores}\n";

            if (TuvoConflictos)
                stats += $"⚠️ Conflictos: {CantidadConflictos}\n";

            if (PorcentajeExito.HasValue)
                stats += $"\n✓ Éxito: {PorcentajeExito:N2}%\n";

            if (VelocidadRegistrosPorSegundo.HasValue)
                stats += $"Velocidad: {VelocidadRegistrosPorSegundo:N2} reg/seg\n";

            if (TamanioDatosMB.HasValue)
                stats += $"Datos: {TamanioDatosMB:N2} MB\n";

            return stats;
        }

        /// <summary>
        /// Valida que la sincronización sea correcta
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(DispositivoId))
                errores.Add("El ID del dispositivo es requerido");

            if (string.IsNullOrWhiteSpace(TipoDispositivo))
                errores.Add("El tipo de dispositivo es requerido");

            if (FechaFin.HasValue && FechaFin.Value < FechaInicio)
                errores.Add("La fecha de fin no puede ser anterior a la fecha de inicio");

            if (DuracionMs.HasValue && DuracionMs < 0)
                errores.Add("La duración no puede ser negativa");

            if (RegistrosCreados < 0 || RegistrosActualizados < 0 || RegistrosEliminados < 0)
                errores.Add("Los contadores de registros no pueden ser negativos");

            if (CantidadErrores < 0 || CantidadConflictos < 0)
                errores.Add("Los contadores de errores y conflictos no pueden ser negativos");

            if (TamanioDatos.HasValue && TamanioDatos < 0)
                errores.Add("El tamaño de datos no puede ser negativo");

            if (Prioridad < 1 || Prioridad > 10)
                errores.Add("La prioridad debe estar entre 1 y 10");

            if (Reintentos < 0)
                errores.Add("Los reintentos no pueden ser negativos");

            return errores;
        }

        #endregion
    }
}