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
    /// Parámetros configurables del sistema
    /// </summary>
    [Table("ParametrosSistema")]
    public class ParametroSistema : BaseEntity, IAuditableEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant) - NULL para parámetros globales
        /// </summary>
        public int? EscuelaId { get; set; }

        /// <summary>
        /// Clave única del parámetro
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Clave { get; set; }

        /// <summary>
        /// Valor del parámetro
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Valor { get; set; }

        /// <summary>
        /// Tipo de dato del parámetro
        /// </summary>
        [Required]
        public TipoParametro TipoDato { get; set; }

        #endregion

        #region Descripción

        /// <summary>
        /// Nombre descriptivo del parámetro
        /// </summary>
        [Required]
        [StringLength(300)]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción del parámetro
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Categoría o módulo del parámetro
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Categoria { get; set; }

        #endregion

        #region Configuración

        /// <summary>
        /// Valor predeterminado
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValorPredeterminado { get; set; }

        /// <summary>
        /// Valores permitidos (JSON array para listas)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValoresPermitidos { get; set; }

        /// <summary>
        /// Valor mínimo (para tipos numéricos)
        /// </summary>
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ValorMinimo { get; set; }

        /// <summary>
        /// Valor máximo (para tipos numéricos)
        /// </summary>
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ValorMaximo { get; set; }

        /// <summary>
        /// Expresión regular de validación
        /// </summary>
        [StringLength(500)]
        public string ExpresionValidacion { get; set; }

        #endregion

        #region Comportamiento

        /// <summary>
        /// Es parámetro global del sistema
        /// </summary>
        public bool EsGlobal { get; set; }

        /// <summary>
        /// Es configurable por el usuario
        /// </summary>
        public bool EsConfigurable { get; set; }

        /// <summary>
        /// Es visible en la interfaz de configuración
        /// </summary>
        public bool EsVisible { get; set; }

        /// <summary>
        /// Es de solo lectura
        /// </summary>
        public bool EsSoloLectura { get; set; }

        /// <summary>
        /// Requiere reinicio del sistema para aplicar cambios
        /// </summary>
        public bool RequiereReinicio { get; set; }

        /// <summary>
        /// Es parámetro sensible (contraseñas, tokens, etc.)
        /// </summary>
        public bool EsSensible { get; set; }

        /// <summary>
        /// Está encriptado
        /// </summary>
        public bool EstaEncriptado { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Grupo de configuración
        /// </summary>
        [StringLength(100)]
        public string Grupo { get; set; }

        /// <summary>
        /// Orden de visualización
        /// </summary>
        public int Orden { get; set; }

        /// <summary>
        /// Etiquetas
        /// </summary>
        [StringLength(500)]
        public string Etiquetas { get; set; }

        /// <summary>
        /// Unidad de medida (si aplica)
        /// </summary>
        [StringLength(50)]
        public string Unidad { get; set; }

        /// <summary>
        /// URL de ayuda o documentación
        /// </summary>
        [StringLength(500)]
        public string UrlAyuda { get; set; }

        #endregion

        #region Estado

        /// <summary>
        /// Indica si está activo
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        /// <summary>
        /// Fecha del último cambio de valor
        /// </summary>
        public DateTime? FechaUltimoCambio { get; set; }

        /// <summary>
        /// Usuario que realizó el último cambio
        /// </summary>
        public int? UsuarioUltimoCambioId { get; set; }

        /// <summary>
        /// Valor anterior (para auditoría)
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValorAnterior { get; set; }

        #endregion

        #region Cache

        /// <summary>
        /// Habilitar caché para este parámetro
        /// </summary>
        public bool HabilitarCache { get; set; }

        /// <summary>
        /// Tiempo de caché en minutos
        /// </summary>
        public int? TiempoCacheMinutos { get; set; }

        #endregion

        #region Observaciones

        /// <summary>
        /// Observaciones o notas
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Observaciones { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Usuario que realizó el último cambio
        /// </summary>
        [ForeignKey("UsuarioUltimoCambioId")]
        public virtual Usuario UsuarioUltimoCambio { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructor

        public ParametroSistema()
        {
            EsGlobal = false;
            EsConfigurable = true;
            EsVisible = true;
            EsSoloLectura = false;
            RequiereReinicio = false;
            EsSensible = false;
            EstaEncriptado = false;
            Activo = true;
            HabilitarCache = true;
            TiempoCacheMinutos = 60;
            Orden = 0;
            TipoDato = TipoParametro.String;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si tiene valor asignado
        /// </summary>
        public bool TieneValor => !string.IsNullOrWhiteSpace(Valor);

        /// <summary>
        /// Indica si usa el valor predeterminado
        /// </summary>
        public bool UsaValorPredeterminado => string.IsNullOrWhiteSpace(Valor) && !string.IsNullOrWhiteSpace(ValorPredeterminado);

        /// <summary>
        /// Obtiene el valor efectivo (valor actual o predeterminado)
        /// </summary>
        public string ValorEfectivo => TieneValor ? Valor : ValorPredeterminado;

        /// <summary>
        /// Indica si es de tipo string
        /// </summary>
        public bool EsString => TipoDato == TipoParametro.String;

        /// <summary>
        /// Indica si es de tipo entero
        /// </summary>
        public bool EsEntero => TipoDato == TipoParametro.Integer;

        /// <summary>
        /// Indica si es de tipo decimal
        /// </summary>
        public bool EsDecimal => TipoDato == TipoParametro.Decimal;

        /// <summary>
        /// Indica si es de tipo booleano
        /// </summary>
        public bool EsBooleano => TipoDato == TipoParametro.Boolean;

        /// <summary>
        /// Indica si es de tipo JSON
        /// </summary>
        public bool EsJSON => TipoDato == TipoParametro.JSON;

        /// <summary>
        /// Indica si es de tipo fecha
        /// </summary>
        public bool EsFecha => TipoDato == TipoParametro.DateTime;

        /// <summary>
        /// Indica si puede ser modificado
        /// </summary>
        public bool PuedeSerModificado => Activo && EsConfigurable && !EsSoloLectura;

        /// <summary>
        /// Indica si tiene restricciones de valor
        /// </summary>
        public bool TieneRestricciones => !string.IsNullOrWhiteSpace(ValoresPermitidos) ||
                                          ValorMinimo.HasValue ||
                                          ValorMaximo.HasValue ||
                                          !string.IsNullOrWhiteSpace(ExpresionValidacion);

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
        /// Indica si ha sido modificado recientemente (últimos 7 días)
        /// </summary>
        public bool ModificadoRecientemente
        {
            get
            {
                var dias = DiasSinCambio;
                return dias.HasValue && dias.Value <= 7;
            }
        }

        /// <summary>
        /// Clave completa (incluye escuela si no es global)
        /// </summary>
        public string ClaveCompleta => EsGlobal ? Clave : $"{EscuelaId}:{Clave}";

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Establece el valor del parámetro
        /// </summary>
        public void EstablecerValor(string nuevoValor, int? usuarioId = null)
        {
            if (!PuedeSerModificado)
                throw new InvalidOperationException("Este parámetro no puede ser modificado");

            if (!ValidarValor(nuevoValor))
                throw new ArgumentException("El valor no es válido para este parámetro");

            ValorAnterior = Valor;
            Valor = nuevoValor;
            FechaUltimoCambio = DateTime.Now;
            UsuarioUltimoCambioId = usuarioId;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Restaura el valor predeterminado
        /// </summary>
        public void RestaurarValorPredeterminado(int? usuarioId = null)
        {
            if (!PuedeSerModificado)
                throw new InvalidOperationException("Este parámetro no puede ser modificado");

            ValorAnterior = Valor;
            Valor = ValorPredeterminado;
            FechaUltimoCambio = DateTime.Now;
            UsuarioUltimoCambioId = usuarioId;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida el valor del parámetro
        /// </summary>
        public bool ValidarValor(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return !string.IsNullOrWhiteSpace(ValorPredeterminado);

            // Validar según tipo de dato
            switch (TipoDato)
            {
                case TipoParametro.Integer:
                    if (!int.TryParse(valor, out var valorInt))
                        return false;
                    if (ValorMinimo.HasValue && valorInt < ValorMinimo.Value)
                        return false;
                    if (ValorMaximo.HasValue && valorInt > ValorMaximo.Value)
                        return false;
                    break;

                case TipoParametro.Decimal:
                    if (!decimal.TryParse(valor, out var valorDecimal))
                        return false;
                    if (ValorMinimo.HasValue && valorDecimal < ValorMinimo.Value)
                        return false;
                    if (ValorMaximo.HasValue && valorDecimal > ValorMaximo.Value)
                        return false;
                    break;

                case TipoParametro.Boolean:
                    if (!bool.TryParse(valor, out _))
                        return false;
                    break;

                case TipoParametro.DateTime:
                    if (!DateTime.TryParse(valor, out _))
                        return false;
                    break;

                case TipoParametro.JSON:
                    // Validación básica de JSON
                    try
                    {
                        System.Text.Json.JsonDocument.Parse(valor);
                    }
                    catch
                    {
                        return false;
                    }
                    break;
            }

            // Validar contra valores permitidos
            if (!string.IsNullOrWhiteSpace(ValoresPermitidos))
            {
                // Aquí se podría deserializar el JSON de valores permitidos y validar
                // Por simplicidad, asumimos que la validación se hace externamente
            }

            // Validar expresión regular
            if (!string.IsNullOrWhiteSpace(ExpresionValidacion))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(valor, ExpresionValidacion))
                    return false;
            }

            return true;
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
        /// Marca el parámetro como sensible
        /// </summary>
        public void MarcarComoSensible(bool encriptar = false)
        {
            EsSensible = true;
            if (encriptar)
                EstaEncriptado = true;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Marca el parámetro como global
        /// </summary>
        public void MarcarComoGlobal()
        {
            EsGlobal = true;
            EscuelaId = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Desactiva el parámetro
        /// </summary>
        public void Desactivar()
        {
            if (Activo)
            {
                Activo = false;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Activa el parámetro
        /// </summary>
        public void Activar()
        {
            if (!Activo)
            {
                Activo = true;
                UpdatedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Configura el caché
        /// </summary>
        public void ConfigurarCache(bool habilitar, int? tiempoMinutos = null)
        {
            HabilitarCache = habilitar;
            if (habilitar)
                TiempoCacheMinutos = tiempoMinutos ?? 60;
            else
                TiempoCacheMinutos = null;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Valida el parámetro
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Clave))
                errores.Add("La clave es requerida");

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(Categoria))
                errores.Add("La categoría es requerida");

            if (EsGlobal && EscuelaId.HasValue)
                errores.Add("Los parámetros globales no deben tener EscuelaId");

            if (!EsGlobal && !EscuelaId.HasValue)
                errores.Add("Los parámetros no globales deben tener EscuelaId");

            if (ValorMinimo.HasValue && ValorMaximo.HasValue && ValorMinimo.Value > ValorMaximo.Value)
                errores.Add("El valor mínimo no puede ser mayor que el máximo");

            if (TieneValor && !ValidarValor(Valor))
                errores.Add("El valor actual no es válido según las restricciones del parámetro");

            if (HabilitarCache && (!TiempoCacheMinutos.HasValue || TiempoCacheMinutos.Value <= 0))
                errores.Add("Si el caché está habilitado, debe especificar el tiempo de caché");

            if (Orden < 0)
                errores.Add("El orden no puede ser negativo");

            return errores;
        }

        #endregion

        #region Métodos Estáticos

        /// <summary>
        /// Crea un parámetro de tipo string
        /// </summary>
        public static ParametroSistema CrearParametroString(int? escuelaId, string clave, string nombre,
                                                            string categoria, string valorPredeterminado = null)
        {
            return new ParametroSistema
            {
                EscuelaId = escuelaId,
                Clave = clave,
                Nombre = nombre,
                Categoria = categoria,
                TipoDato = TipoParametro.String,
                ValorPredeterminado = valorPredeterminado,
                EsGlobal = !escuelaId.HasValue
            };
        }

        /// <summary>
        /// Crea un parámetro de tipo entero
        /// </summary>
        public static ParametroSistema CrearParametroInt(int? escuelaId, string clave, string nombre,
                                                         string categoria, int? valorPredeterminado = null,
                                                         int? minimo = null, int? maximo = null)
        {
            return new ParametroSistema
            {
                EscuelaId = escuelaId,
                Clave = clave,
                Nombre = nombre,
                Categoria = categoria,
                TipoDato = TipoParametro.Integer,
                ValorPredeterminado = valorPredeterminado?.ToString(),
                ValorMinimo = minimo,
                ValorMaximo = maximo,
                EsGlobal = !escuelaId.HasValue
            };
        }

        /// <summary>
        /// Crea un parámetro de tipo booleano
        /// </summary>
        public static ParametroSistema CrearParametroBool(int? escuelaId, string clave, string nombre,
                                                          string categoria, bool valorPredeterminado = false)
        {
            return new ParametroSistema
            {
                EscuelaId = escuelaId,
                Clave = clave,
                Nombre = nombre,
                Categoria = categoria,
                TipoDato = TipoParametro.Boolean,
                ValorPredeterminado = valorPredeterminado.ToString(),
                EsGlobal = !escuelaId.HasValue
            };
        }

        /// <summary>
        /// Crea un parámetro de tipo decimal
        /// </summary>
        public static ParametroSistema CrearParametroDecimal(int? escuelaId, string clave, string nombre,
                                                             string categoria, decimal? valorPredeterminado = null,
                                                             decimal? minimo = null, decimal? maximo = null)
        {
            return new ParametroSistema
            {
                EscuelaId = escuelaId,
                Clave = clave,
                Nombre = nombre,
                Categoria = categoria,
                TipoDato = TipoParametro.Decimal,
                ValorPredeterminado = valorPredeterminado?.ToString(),
                ValorMinimo = minimo,
                ValorMaximo = maximo,
                EsGlobal = !escuelaId.HasValue
            };
        }

        #endregion
    }
}