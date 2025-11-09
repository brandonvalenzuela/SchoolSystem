using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Configuracion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Configuracion
{
    /// <summary>
    /// Preferencias personalizadas de usuarios
    /// </summary>
    [Table("PreferenciasUsuario")]
    public class PreferenciaUsuario : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        public int? EscuelaId { get; set; }

        /// <summary>
        /// Usuario al que pertenece la preferencia
        /// </summary>
        [Required]
        public int UsuarioId { get; set; }

        /// <summary>
        /// Clave de la preferencia
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Clave { get; set; }

        /// <summary>
        /// Valor de la preferencia
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Valor { get; set; }

        /// <summary>
        /// Tipo de preferencia
        /// </summary>
        [Required]
        public TipoPreferencia Tipo { get; set; }

        #endregion

        #region Descripción

        /// <summary>
        /// Nombre descriptivo de la preferencia
        /// </summary>
        [StringLength(300)]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción de la preferencia
        /// </summary>
        [StringLength(500)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Categoría de la preferencia
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Categoria { get; set; }

        #endregion

        #region Configuración

        /// <summary>
        /// Grupo de preferencias
        /// </summary>
        [StringLength(100)]
        public string Grupo { get; set; }

        /// <summary>
        /// Tipo de dato del valor
        /// </summary>
        [StringLength(50)]
        public string TipoDato { get; set; }

        /// <summary>
        /// Valor predeterminado
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValorPredeterminado { get; set; }

        #endregion

        #region Sincronización

        /// <summary>
        /// Es sincronizable entre dispositivos
        /// </summary>
        public bool EsSincronizable { get; set; }

        /// <summary>
        /// Fecha de última sincronización
        /// </summary>
        public DateTime? FechaUltimaSincronizacion { get; set; }

        /// <summary>
        /// Dispositivo de origen
        /// </summary>
        [StringLength(200)]
        public string DispositivoOrigen { get; set; }

        /// <summary>
        /// Hash de sincronización
        /// </summary>
        [StringLength(500)]
        public string HashSincronizacion { get; set; }

        #endregion

        #region Alcance

        /// <summary>
        /// Alcance de la preferencia
        /// </summary>
        [Required]
        public AlcancePreferencia Alcance { get; set; }

        /// <summary>
        /// Es preferencia del sistema (no editable por usuario)
        /// </summary>
        public bool EsPreferenciaSistema { get; set; }

        /// <summary>
        /// Es privada (solo visible para el usuario)
        /// </summary>
        public bool EsPrivada { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Indica si está activa
        /// </summary>
        [Required]
        public bool Activa { get; set; }

        /// <summary>
        /// Fecha del último cambio
        /// </summary>
        public DateTime? FechaUltimoCambio { get; set; }

        /// <summary>
        /// Valor anterior (para rollback)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValorAnterior { get; set; }

        #endregion

        #region Validación

        /// <summary>
        /// Requiere validación
        /// </summary>
        public bool RequiereValidacion { get; set; }

        /// <summary>
        /// Expresión de validación
        /// </summary>
        [StringLength(500)]
        public string ExpresionValidacion { get; set; }

        /// <summary>
        /// Valores permitidos (JSON array)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValoresPermitidos { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Etiquetas
        /// </summary>
        [StringLength(500)]
        public string Etiquetas { get; set; }

        /// <summary>
        /// Orden de visualización
        /// </summary>
        public int Orden { get; set; }

        /// <summary>
        /// Icono
        /// </summary>
        [StringLength(50)]
        public string Icono { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Usuario relacionado
        /// </summary>
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public PreferenciaUsuario()
        {
            Tipo = TipoPreferencia.Personalizacion;
            Alcance = AlcancePreferencia.Usuario;
            EsSincronizable = true;
            EsPreferenciaSistema = false;
            EsPrivada = false;
            Activa = true;
            RequiereValidacion = false;
            Orden = 0;
            TipoDato = "String";
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si tiene valor asignado
        /// </summary>
        public bool TieneValor => !string.IsNullOrWhiteSpace(Valor);

        /// <summary>
        /// Obtiene el valor efectivo (valor actual o predeterminado)
        /// </summary>
        public string ValorEfectivo => TieneValor ? Valor : ValorPredeterminado;

        /// <summary>
        /// Indica si es preferencia de tema
        /// </summary>
        public bool EsTema => Tipo == TipoPreferencia.Tema;

        /// <summary>
        /// Indica si es preferencia de notificaciones
        /// </summary>
        public bool EsNotificacion => Tipo == TipoPreferencia.Notificaciones;

        /// <summary>
        /// Indica si es preferencia de privacidad
        /// </summary>
        public bool EsPrivacidad => Tipo == TipoPreferencia.Privacidad;

        /// <summary>
        /// Indica si puede ser sincronizada
        /// </summary>
        public bool PuedeSincronizarse => EsSincronizable && Activa;

        /// <summary>
        /// Indica si fue sincronizada
        /// </summary>
        public bool FueSincronizada => FechaUltimaSincronizacion.HasValue;

        /// <summary>
        /// Días desde la última sincronización
        /// </summary>
        public int? DiasSinSincronizar
        {
            get
            {
                if (!FechaUltimaSincronizacion.HasValue) return null;
                return (DateTime.Now.Date - FechaUltimaSincronizacion.Value.Date).Days;
            }
        }

        /// <summary>
        /// Indica si puede ser modificada
        /// </summary>
        public bool PuedeSerModificada => Activa && !EsPreferenciaSistema;

        /// <summary>
        /// Días desde el último cambio
        /// </summary>
        public int? DiasSinCambio
        {
            get
            {
                if (!FechaUltimoCambio.HasValue) return null;
                return (DateTime.Now.Date - FechaUltimoCambio.Value.Date).Days;
            }
        }

        /// <summary>
        /// Indica si fue modificada recientemente (últimos 7 días)
        /// </summary>
        public bool ModificadaRecientemente
        {
            get
            {
                var dias = DiasSinCambio;
                return dias.HasValue && dias.Value <= 7;
            }
        }

        /// <summary>
        /// Clave completa (usuario + clave)
        /// </summary>
        public string ClaveCompleta => $"{UsuarioId}:{Clave}";

        /// <summary>
        /// Indica si es alcance global
        /// </summary>
        public bool EsAlcanceGlobal => Alcance == AlcancePreferencia.Global;

        /// <summary>
        /// Indica si es alcance de escuela
        /// </summary>
        public bool EsAlcanceEscuela => Alcance == AlcancePreferencia.Escuela;

        /// <summary>
        /// Indica si es alcance de usuario
        /// </summary>
        public bool EsAlcanceUsuario => Alcance == AlcancePreferencia.Usuario;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Establece el valor de la preferencia
        /// </summary>
        public void EstablecerValor(string nuevoValor)
        {
            if (!PuedeSerModificada)
                throw new InvalidOperationException("Esta preferencia no puede ser modificada");

            if (RequiereValidacion && !ValidarValor(nuevoValor))
                throw new ArgumentException("El valor no es válido para esta preferencia");

            ValorAnterior = Valor;
            Valor = nuevoValor;
            FechaUltimoCambio = DateTime.Now;
            UpdatedAt = DateTime.Now;

            // Actualizar hash de sincronización
            if (EsSincronizable)
                ActualizarHashSincronizacion();
        }

        /// <summary>
        /// Restaura el valor predeterminado
        /// </summary>
        public void RestaurarValorPredeterminado()
        {
            if (!PuedeSerModificada)
                throw new InvalidOperationException("Esta preferencia no puede ser modificada");

            ValorAnterior = Valor;
            Valor = ValorPredeterminado;
            FechaUltimoCambio = DateTime.Now;
            UpdatedAt = DateTime.Now;

            if (EsSincronizable)
                ActualizarHashSincronizacion();
        }

        /// <summary>
        /// Revierte al valor anterior
        /// </summary>
        public void RevertirCambio()
        {
            if (string.IsNullOrWhiteSpace(ValorAnterior))
                throw new InvalidOperationException("No hay valor anterior para revertir");

            var temp = Valor;
            Valor = ValorAnterior;
            ValorAnterior = temp;
            FechaUltimoCambio = DateTime.Now;
            UpdatedAt = DateTime.Now;

            if (EsSincronizable)
                ActualizarHashSincronizacion();
        }

        /// <summary>
        /// Valida el valor de la preferencia
        /// </summary>
        public bool ValidarValor(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return !string.IsNullOrWhiteSpace(ValorPredeterminado);

            // Validar según tipo de dato
            switch (TipoDato?.ToLower())
            {
                case "int":
                case "integer":
                    return int.TryParse(valor, out _);

                case "decimal":
                case "double":
                    return decimal.TryParse(valor, out _);

                case "bool":
                case "boolean":
                    return bool.TryParse(valor, out _);

                case "datetime":
                case "date":
                    return DateTime.TryParse(valor, out _);

                case "json":
                    try
                    {
                        System.Text.Json.JsonDocument.Parse(valor);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }

                case "email":
                    return System.Text.RegularExpressions.Regex.IsMatch(valor,
                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

                case "url":
                    return Uri.TryCreate(valor, UriKind.Absolute, out _);

                case "color":
                    return System.Text.RegularExpressions.Regex.IsMatch(valor,
                        @"^#[0-9A-Fa-f]{6}$");
            }

            // Validar expresión regular
            if (!string.IsNullOrWhiteSpace(ExpresionValidacion))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(valor, ExpresionValidacion))
                    return false;
            }

            // Validar valores permitidos
            if (!string.IsNullOrWhiteSpace(ValoresPermitidos))
            {
                // Aquí se podría deserializar y validar contra la lista
                // Por simplicidad, asumimos que se valida externamente
            }

            return true;
        }

        /// <summary>
        /// Marca la preferencia como sincronizada
        /// </summary>
        public void MarcarComoSincronizada(string dispositivoOrigen = null)
        {
            FechaUltimaSincronizacion = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(dispositivoOrigen))
                DispositivoOrigen = dispositivoOrigen;

            ActualizarHashSincronizacion();
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Actualiza el hash de sincronización
        /// </summary>
        private void ActualizarHashSincronizacion()
        {
            if (!EsSincronizable) return;

            var contenido = $"{UsuarioId}:{Clave}:{Valor}:{DateTime.Now.Ticks}";
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(contenido);
                var hash = sha256.ComputeHash(bytes);
                HashSincronizacion = Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Activa la preferencia
        /// </summary>
        public void Activar()
        {
            if (!Activa)
            {
                Activa = true;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Desactiva la preferencia
        /// </summary>
        public void Desactivar()
        {
            if (Activa)
            {
                Activa = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Obtiene el valor como entero
        /// </summary>
        public int? ObtenerValorComoInt()
        {
            var valor = ValorEfectivo;
            if (string.IsNullOrWhiteSpace(valor)) return null;
            return int.TryParse(valor, out var result) ? result : null;
        }

        /// <summary>
        /// Obtiene el valor como decimal
        /// </summary>
        public decimal? ObtenerValorComoDecimal()
        {
            var valor = ValorEfectivo;
            if (string.IsNullOrWhiteSpace(valor)) return null;
            return decimal.TryParse(valor, out var result) ? result : null;
        }

        /// <summary>
        /// Obtiene el valor como booleano
        /// </summary>
        public bool? ObtenerValorComoBool()
        {
            var valor = ValorEfectivo;
            if (string.IsNullOrWhiteSpace(valor)) return null;
            return bool.TryParse(valor, out var result) ? result : null;
        }

        /// <summary>
        /// Obtiene el valor como fecha
        /// </summary>
        public DateTime? ObtenerValorComoFecha()
        {
            var valor = ValorEfectivo;
            if (string.IsNullOrWhiteSpace(valor)) return null;
            return DateTime.TryParse(valor, out var result) ? result : null;
        }

        /// <summary>
        /// Clona la preferencia para otro usuario
        /// </summary>
        public PreferenciaUsuario ClonarParaUsuario(int nuevoUsuarioId)
        {
            return new PreferenciaUsuario
            {
                EscuelaId = this.EscuelaId,
                UsuarioId = nuevoUsuarioId,
                Clave = this.Clave,
                Valor = this.Valor,
                Tipo = this.Tipo,
                Nombre = this.Nombre,
                Descripcion = this.Descripcion,
                Categoria = this.Categoria,
                Grupo = this.Grupo,
                TipoDato = this.TipoDato,
                ValorPredeterminado = this.ValorPredeterminado,
                EsSincronizable = this.EsSincronizable,
                Alcance = this.Alcance,
                EsPreferenciaSistema = this.EsPreferenciaSistema,
                EsPrivada = this.EsPrivada,
                RequiereValidacion = this.RequiereValidacion,
                ExpresionValidacion = this.ExpresionValidacion,
                ValoresPermitidos = this.ValoresPermitidos,
                Etiquetas = this.Etiquetas,
                Orden = this.Orden,
                Icono = this.Icono
            };
        }

        /// <summary>
        /// Valida la preferencia
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (UsuarioId <= 0)
                errores.Add("El usuario es requerido");

            if (string.IsNullOrWhiteSpace(Clave))
                errores.Add("La clave es requerida");

            if (string.IsNullOrWhiteSpace(Categoria))
                errores.Add("La categoría es requerida");

            if (TieneValor && RequiereValidacion && !ValidarValor(Valor))
                errores.Add("El valor actual no es válido");

            if (Orden < 0)
                errores.Add("El orden no puede ser negativo");

            if (EsAlcanceEscuela && !EscuelaId.HasValue)
                errores.Add("Las preferencias con alcance de escuela deben tener EscuelaId");

            if (EsAlcanceGlobal && EscuelaId.HasValue)
                errores.Add("Las preferencias con alcance global no deben tener EscuelaId");

            return errores;
        }

        #endregion

        #region Métodos Estáticos

        /// <summary>
        /// Crea una preferencia de tema
        /// </summary>
        public static PreferenciaUsuario CrearPreferenciaTema(int usuarioId, int? escuelaId,
                                                              string clave, string valor)
        {
            return new PreferenciaUsuario
            {
                UsuarioId = usuarioId,
                EscuelaId = escuelaId,
                Clave = clave,
                Valor = valor,
                Tipo = TipoPreferencia.Tema,
                Categoria = "Apariencia",
                EsSincronizable = true,
                Alcance = AlcancePreferencia.Usuario
            };
        }

        /// <summary>
        /// Crea una preferencia de notificaciones
        /// </summary>
        public static PreferenciaUsuario CrearPreferenciaNotificacion(int usuarioId, int? escuelaId,
                                                                      string clave, bool valor)
        {
            return new PreferenciaUsuario
            {
                UsuarioId = usuarioId,
                EscuelaId = escuelaId,
                Clave = clave,
                Valor = valor.ToString(),
                TipoDato = "Boolean",
                Tipo = TipoPreferencia.Notificaciones,
                Categoria = "Notificaciones",
                EsSincronizable = true,
                Alcance = AlcancePreferencia.Usuario
            };
        }

        /// <summary>
        /// Crea una preferencia de privacidad
        /// </summary>
        public static PreferenciaUsuario CrearPreferenciaPrivacidad(int usuarioId, int? escuelaId,
                                                                    string clave, string valor)
        {
            return new PreferenciaUsuario
            {
                UsuarioId = usuarioId,
                EscuelaId = escuelaId,
                Clave = clave,
                Valor = valor,
                Tipo = TipoPreferencia.Privacidad,
                Categoria = "Privacidad",
                EsPrivada = true,
                EsSincronizable = true,
                Alcance = AlcancePreferencia.Usuario
            };
        }

        #endregion
    }
}