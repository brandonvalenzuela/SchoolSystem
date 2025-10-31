using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Auditoria;
using SchoolSystem.Domain.Enums.Conducta;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Auditoria
{
    /// <summary>
    /// Registro de auditoría de todas las acciones del sistema
    /// </summary>
    [Table("LogsAuditoria")]
    public class LogAuditoria : BaseEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        public int? EscuelaId { get; set; }

        /// <summary>
        /// Usuario que realizó la acción
        /// </summary>
        public int? UsuarioId { get; set; }

        /// <summary>
        /// Nombre del usuario (por si se elimina el usuario)
        /// </summary>
        [StringLength(200)]
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Email del usuario
        /// </summary>
        [StringLength(200)]
        public string EmailUsuario { get; set; }

        #endregion

        #region Acción

        /// <summary>
        /// Tipo de acción realizada
        /// </summary>
        [Required]
        public TipoAccion TipoAccion { get; set; }

        /// <summary>
        /// Descripción de la acción
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Fecha y hora de la acción
        /// </summary>
        [Required]
        public DateTime FechaHora { get; set; }

        #endregion

        #region Entidad Afectada

        /// <summary>
        /// Nombre de la entidad/tabla afectada
        /// </summary>
        [StringLength(100)]
        public string EntidadAfectada { get; set; }

        /// <summary>
        /// ID de la entidad afectada
        /// </summary>
        public int? EntidadAfectadaId { get; set; }

        /// <summary>
        /// Tipo de entidad (para polimorfismo)
        /// </summary>
        [StringLength(100)]
        public string TipoEntidad { get; set; }

        #endregion

        #region Cambios

        /// <summary>
        /// Valores anteriores en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValoresAnteriores { get; set; }

        /// <summary>
        /// Valores nuevos en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValoresNuevos { get; set; }

        /// <summary>
        /// Campos modificados (separados por coma)
        /// </summary>
        [StringLength(500)]
        public string CamposModificados { get; set; }

        #endregion

        #region Información Técnica

        /// <summary>
        /// Dirección IP desde donde se realizó la acción
        /// </summary>
        [StringLength(45)]
        public string DireccionIP { get; set; }

        /// <summary>
        /// User Agent del navegador/cliente
        /// </summary>
        [StringLength(500)]
        public string UserAgent { get; set; }

        /// <summary>
        /// Navegador
        /// </summary>
        [StringLength(100)]
        public string Navegador { get; set; }

        /// <summary>
        /// Sistema operativo
        /// </summary>
        [StringLength(100)]
        public string SistemaOperativo { get; set; }

        /// <summary>
        /// Dispositivo (móvil, desktop, tablet)
        /// </summary>
        [StringLength(50)]
        public string Dispositivo { get; set; }

        #endregion

        #region Resultado

        /// <summary>
        /// Indica si la acción fue exitosa
        /// </summary>
        [Required]
        public bool Exitoso { get; set; }

        /// <summary>
        /// Código de resultado HTTP (si aplica)
        /// </summary>
        public int? CodigoResultado { get; set; }

        /// <summary>
        /// Mensaje de error (si falló)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string MensajeError { get; set; }

        /// <summary>
        /// Stack trace del error (si falló)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string StackTrace { get; set; }

        #endregion

        #region Rendimiento

        /// <summary>
        /// Duración de la operación en milisegundos
        /// </summary>
        public int? DuracionMs { get; set; }

        #endregion

        #region Módulo y Funcionalidad

        /// <summary>
        /// Módulo del sistema donde se realizó la acción
        /// </summary>
        [StringLength(100)]
        public string Modulo { get; set; }

        /// <summary>
        /// Funcionalidad específica
        /// </summary>
        [StringLength(200)]
        public string Funcionalidad { get; set; }

        /// <summary>
        /// Controlador/Servicio que procesó la acción
        /// </summary>
        [StringLength(200)]
        public string Controlador { get; set; }

        /// <summary>
        /// Acción/Método específico
        /// </summary>
        [StringLength(200)]
        public string Metodo { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Datos adicionales en formato JSON
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string DatosAdicionales { get; set; }

        /// <summary>
        /// Nivel de severidad
        /// </summary>
        public NivelSeveridad? Severidad { get; set; }

        /// <summary>
        /// Tags o etiquetas
        /// </summary>
        [StringLength(500)]
        public string Tags { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Usuario relacionado
        /// </summary>
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        #endregion

        #region Constructor

        public LogAuditoria()
        {
            FechaHora = DateTime.Now;
            Exitoso = true;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si es una acción de creación
        /// </summary>
        public bool EsCreacion => TipoAccion == TipoAccion.Crear;

        /// <summary>
        /// Indica si es una acción de actualización
        /// </summary>
        public bool EsActualizacion => TipoAccion == TipoAccion.Actualizar;

        /// <summary>
        /// Indica si es una acción de eliminación
        /// </summary>
        public bool EsEliminacion => TipoAccion == TipoAccion.Eliminar;

        /// <summary>
        /// Indica si es un inicio de sesión
        /// </summary>
        public bool EsLogin => TipoAccion == TipoAccion.Login;

        /// <summary>
        /// Indica si tuvo error
        /// </summary>
        public bool TuvoError => !Exitoso;

        /// <summary>
        /// Indica si tiene cambios registrados
        /// </summary>
        public bool TieneCambios => !string.IsNullOrWhiteSpace(ValoresAnteriores) ||
                                    !string.IsNullOrWhiteSpace(ValoresNuevos);

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
        /// Indica si es una operación lenta (más de 3 segundos)
        /// </summary>
        public bool EsOperacionLenta => DuracionMs.HasValue && DuracionMs.Value > 3000;

        /// <summary>
        /// Indica si es crítico
        /// </summary>
        public bool EsCritico => Severidad == NivelSeveridad.Critico || Severidad == NivelSeveridad.Alto;

        /// <summary>
        /// Cantidad de campos modificados
        /// </summary>
        public int CantidadCamposModificados
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CamposModificados)) return 0;
                return CamposModificados.Split(',').Length;
            }
        }

        /// <summary>
        /// Resumen de la acción
        /// </summary>
        public string Resumen
        {
            get
            {
                var resumen = $"{TipoAccion}";

                if (!string.IsNullOrWhiteSpace(EntidadAfectada))
                    resumen += $" en {EntidadAfectada}";

                if (EntidadAfectadaId.HasValue)
                    resumen += $" #{EntidadAfectadaId}";

                if (!string.IsNullOrWhiteSpace(NombreUsuario))
                    resumen += $" por {NombreUsuario}";

                return resumen;
            }
        }

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Marca la acción como fallida
        /// </summary>
        public void MarcarComoFallida(string mensajeError, string stackTrace = null, int? codigoResultado = null)
        {
            Exitoso = false;
            MensajeError = mensajeError;
            StackTrace = stackTrace;
            CodigoResultado = codigoResultado;
        }

        /// <summary>
        /// Establece la duración de la operación
        /// </summary>
        public void EstablecerDuracion(int milisegundos)
        {
            if (milisegundos < 0)
                throw new ArgumentException("La duración no puede ser negativa");

            DuracionMs = milisegundos;
        }

        /// <summary>
        /// Establece información del usuario
        /// </summary>
        public void EstablecerUsuario(int? usuarioId, string nombreUsuario, string emailUsuario)
        {
            UsuarioId = usuarioId;
            NombreUsuario = nombreUsuario;
            EmailUsuario = emailUsuario;
        }

        /// <summary>
        /// Establece la entidad afectada
        /// </summary>
        public void EstablecerEntidadAfectada(string nombreEntidad, int? entidadId, string tipoEntidad = null)
        {
            EntidadAfectada = nombreEntidad;
            EntidadAfectadaId = entidadId;
            TipoEntidad = tipoEntidad ?? nombreEntidad;
        }

        /// <summary>
        /// Registra los cambios realizados
        /// </summary>
        public void RegistrarCambios(string valoresAnteriores, string valoresNuevos, string camposModificados = null)
        {
            ValoresAnteriores = valoresAnteriores;
            ValoresNuevos = valoresNuevos;
            CamposModificados = camposModificados;
        }

        /// <summary>
        /// Establece información técnica
        /// </summary>
        public void EstablecerInformacionTecnica(string ip, string userAgent, string navegador = null,
                                                 string sistemaOperativo = null, string dispositivo = null)
        {
            DireccionIP = ip;
            UserAgent = userAgent;
            Navegador = navegador;
            SistemaOperativo = sistemaOperativo;
            Dispositivo = dispositivo;
        }

        /// <summary>
        /// Establece el módulo y funcionalidad
        /// </summary>
        public void EstablecerModulo(string modulo, string funcionalidad = null, string controlador = null, string metodo = null)
        {
            Modulo = modulo;
            Funcionalidad = funcionalidad;
            Controlador = controlador;
            Metodo = metodo;
        }

        /// <summary>
        /// Agrega datos adicionales
        /// </summary>
        public void AgregarDatosAdicionales(string datosJSON)
        {
            DatosAdicionales = datosJSON;
        }

        /// <summary>
        /// Establece la severidad
        /// </summary>
        public void EstablecerSeveridad(NivelSeveridad severidad)
        {
            Severidad = severidad;
        }

        /// <summary>
        /// Valida que el log sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Descripcion))
                errores.Add("La descripción es requerida");

            if (FechaHora > DateTime.Now)
                errores.Add("La fecha/hora no puede ser futura");

            if (DuracionMs.HasValue && DuracionMs < 0)
                errores.Add("La duración no puede ser negativa");

            if (!Exitoso && string.IsNullOrWhiteSpace(MensajeError))
                errores.Add("Si la acción falló, debe tener un mensaje de error");

            if (CodigoResultado.HasValue && (CodigoResultado < 100 || CodigoResultado > 599))
                errores.Add("El código de resultado HTTP debe estar entre 100 y 599");

            return errores;
        }

        #endregion

        #region Métodos Estáticos de Creación

        /// <summary>
        /// Crea un log de auditoría para una creación
        /// </summary>
        public static LogAuditoria CrearLogCreacion(int? escuelaId, int? usuarioId, string nombreUsuario,
                                                    string entidad, int entidadId, string valoresNuevos, string descripcion = null)
        {
            return new LogAuditoria
            {
                EscuelaId = escuelaId,
                UsuarioId = usuarioId,
                NombreUsuario = nombreUsuario,
                TipoAccion = TipoAccion.Crear,
                Descripcion = descripcion ?? $"Creación de {entidad}",
                EntidadAfectada = entidad,
                EntidadAfectadaId = entidadId,
                ValoresNuevos = valoresNuevos,
                Exitoso = true
            };
        }

        /// <summary>
        /// Crea un log de auditoría para una actualización
        /// </summary>
        public static LogAuditoria CrearLogActualizacion(int? escuelaId, int? usuarioId, string nombreUsuario,
                                                         string entidad, int entidadId, string valoresAnteriores,
                                                         string valoresNuevos, string camposModificados, string descripcion = null)
        {
            return new LogAuditoria
            {
                EscuelaId = escuelaId,
                UsuarioId = usuarioId,
                NombreUsuario = nombreUsuario,
                TipoAccion = TipoAccion.Actualizar,
                Descripcion = descripcion ?? $"Actualización de {entidad}",
                EntidadAfectada = entidad,
                EntidadAfectadaId = entidadId,
                ValoresAnteriores = valoresAnteriores,
                ValoresNuevos = valoresNuevos,
                CamposModificados = camposModificados,
                Exitoso = true
            };
        }

        /// <summary>
        /// Crea un log de auditoría para una eliminación
        /// </summary>
        public static LogAuditoria CrearLogEliminacion(int? escuelaId, int? usuarioId, string nombreUsuario,
                                                       string entidad, int entidadId, string valoresAnteriores, string descripcion = null)
        {
            return new LogAuditoria
            {
                EscuelaId = escuelaId,
                UsuarioId = usuarioId,
                NombreUsuario = nombreUsuario,
                TipoAccion = TipoAccion.Eliminar,
                Descripcion = descripcion ?? $"Eliminación de {entidad}",
                EntidadAfectada = entidad,
                EntidadAfectadaId = entidadId,
                ValoresAnteriores = valoresAnteriores,
                Exitoso = true
            };
        }

        /// <summary>
        /// Crea un log de auditoría para un login
        /// </summary>
        public static LogAuditoria CrearLogLogin(int? escuelaId, int? usuarioId, string nombreUsuario,
                                                string email, bool exitoso, string ip, string userAgent)
        {
            return new LogAuditoria
            {
                EscuelaId = escuelaId,
                UsuarioId = usuarioId,
                NombreUsuario = nombreUsuario,
                EmailUsuario = email,
                TipoAccion = TipoAccion.Login,
                Descripcion = exitoso ? "Inicio de sesión exitoso" : "Intento de inicio de sesión fallido",
                DireccionIP = ip,
                UserAgent = userAgent,
                Exitoso = exitoso,
                Modulo = "Autenticación"
            };
        }

        /// <summary>
        /// Crea un log de auditoría para un logout
        /// </summary>
        public static LogAuditoria CrearLogLogout(int? escuelaId, int usuarioId, string nombreUsuario)
        {
            return new LogAuditoria
            {
                EscuelaId = escuelaId,
                UsuarioId = usuarioId,
                NombreUsuario = nombreUsuario,
                TipoAccion = TipoAccion.Logout,
                Descripcion = "Cierre de sesión",
                Exitoso = true,
                Modulo = "Autenticación"
            };
        }

        #endregion
    }
}