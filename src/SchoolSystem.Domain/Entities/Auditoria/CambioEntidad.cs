using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Auditoria
{
    /// <summary>
    /// Historial detallado de cambios en entidades
    /// </summary>
    [Table("CambiosEntidad")]
    public class CambioEntidad : BaseEntity
    {
        #region Propiedades Principales

        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        public int? EscuelaId { get; set; }

        /// <summary>
        /// Log de auditoría asociado
        /// </summary>
        public int? LogAuditoriaId { get; set; }

        /// <summary>
        /// Nombre de la entidad modificada
        /// </summary>
        [Required]
        [StringLength(100)]
        public string NombreEntidad { get; set; }

        /// <summary>
        /// ID de la entidad modificada
        /// </summary>
        [Required]
        public int EntidadId { get; set; }

        #endregion

        #region Campo Modificado

        /// <summary>
        /// Nombre del campo modificado
        /// </summary>
        [Required]
        [StringLength(100)]
        public string NombreCampo { get; set; }

        /// <summary>
        /// Nombre descriptivo del campo
        /// </summary>
        [StringLength(200)]
        public string NombreDescriptivo { get; set; }

        /// <summary>
        /// Tipo de dato del campo
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TipoDato { get; set; }

        #endregion

        #region Valores

        /// <summary>
        /// Valor anterior del campo
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValorAnterior { get; set; }

        /// <summary>
        /// Valor nuevo del campo
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ValorNuevo { get; set; }

        /// <summary>
        /// Valor anterior formateado (para display)
        /// </summary>
        [StringLength(500)]
        public string ValorAnteriorFormateado { get; set; }

        /// <summary>
        /// Valor nuevo formateado (para display)
        /// </summary>
        [StringLength(500)]
        public string ValorNuevoFormateado { get; set; }

        #endregion

        #region Usuario y Fecha

        /// <summary>
        /// Usuario que realizó el cambio
        /// </summary>
        public int? UsuarioId { get; set; }

        /// <summary>
        /// Nombre del usuario
        /// </summary>
        [StringLength(200)]
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Fecha y hora del cambio
        /// </summary>
        [Required]
        public DateTime FechaCambio { get; set; }

        #endregion

        #region Metadata

        /// <summary>
        /// Indica si es un campo sensible (contraseña, datos personales, etc.)
        /// </summary>
        public bool EsCampoSensible { get; set; }

        /// <summary>
        /// Categoría del cambio
        /// </summary>
        [StringLength(100)]
        public string Categoria { get; set; }

        /// <summary>
        /// Etiquetas
        /// </summary>
        [StringLength(500)]
        public string Etiquetas { get; set; }

        /// <summary>
        /// Notas adicionales sobre el cambio
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Notas { get; set; }

        #endregion

        #region Relaciones (Navigation Properties)

        /// <summary>
        /// Log de auditoría relacionado
        /// </summary>
        [ForeignKey("LogAuditoriaId")]
        public virtual LogAuditoria LogAuditoria { get; set; }

        /// <summary>
        /// Usuario relacionado
        /// </summary>
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        #endregion

        #region Constructor

        public CambioEntidad()
        {
            FechaCambio = DateTime.Now;
            EsCampoSensible = false;
        }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Indica si el valor cambió
        /// </summary>
        public bool ValorCambio => ValorAnterior != ValorNuevo;

        /// <summary>
        /// Indica si es un valor nulo a no nulo
        /// </summary>
        public bool EsNuloANoNulo => string.IsNullOrEmpty(ValorAnterior) && !string.IsNullOrEmpty(ValorNuevo);

        /// <summary>
        /// Indica si es un valor no nulo a nulo
        /// </summary>
        public bool EsNoNuloANulo => !string.IsNullOrEmpty(ValorAnterior) && string.IsNullOrEmpty(ValorNuevo);

        /// <summary>
        /// Indica si ambos valores son nulos
        /// </summary>
        public bool AmbosNulos => string.IsNullOrEmpty(ValorAnterior) && string.IsNullOrEmpty(ValorNuevo);

        /// <summary>
        /// Indica si es un campo numérico
        /// </summary>
        public bool EsCampoNumerico
        {
            get
            {
                var tiposNumericos = new[] { "int", "long", "decimal", "double", "float", "short", "byte" };
                return Array.Exists(tiposNumericos, t => TipoDato.ToLower().Contains(t));
            }
        }

        /// <summary>
        /// Indica si es un campo de fecha
        /// </summary>
        public bool EsCampoFecha => TipoDato.ToLower().Contains("datetime") || TipoDato.ToLower().Contains("date");

        /// <summary>
        /// Indica si es un campo booleano
        /// </summary>
        public bool EsCampoBooleano => TipoDato.ToLower().Contains("bool");

        /// <summary>
        /// Diferencia numérica (si aplica)
        /// </summary>
        public decimal? DiferenciaNumerica
        {
            get
            {
                if (!EsCampoNumerico) return null;

                if (decimal.TryParse(ValorAnterior, out var anterior) &&
                    decimal.TryParse(ValorNuevo, out var nuevo))
                {
                    return nuevo - anterior;
                }

                return null;
            }
        }

        /// <summary>
        /// Porcentaje de cambio (si aplica)
        /// </summary>
        public decimal? PorcentajeCambio
        {
            get
            {
                if (!EsCampoNumerico) return null;

                if (decimal.TryParse(ValorAnterior, out var anterior) &&
                    decimal.TryParse(ValorNuevo, out var nuevo) &&
                    anterior != 0)
                {
                    return ((nuevo - anterior) / anterior) * 100;
                }

                return null;
            }
        }

        /// <summary>
        /// Descripción del cambio
        /// </summary>
        public string DescripcionCambio
        {
            get
            {
                var nombreCampo = NombreDescriptivo ?? NombreCampo;
                var valorAnt = ValorAnteriorFormateado ?? ValorAnterior ?? "(vacío)";
                var valorNvo = ValorNuevoFormateado ?? ValorNuevo ?? "(vacío)";

                if (EsCampoSensible)
                    return $"{nombreCampo}: Valor modificado (campo sensible)";

                return $"{nombreCampo}: {valorAnt} → {valorNvo}";
            }
        }

        /// <summary>
        /// Días desde el cambio
        /// </summary>
        public int DiasDesdeElCambio => (DateTime.Now.Date - FechaCambio.Date).Days;

        #endregion

        #region Métodos de Negocio

        /// <summary>
        /// Establece los valores del cambio
        /// </summary>
        public void EstablecerValores(string valorAnterior, string valorNuevo,
                                      string valorAnteriorFormateado = null, string valorNuevoFormateado = null)
        {
            ValorAnterior = valorAnterior;
            ValorNuevo = valorNuevo;
            ValorAnteriorFormateado = valorAnteriorFormateado;
            ValorNuevoFormateado = valorNuevoFormateado;
        }

        /// <summary>
        /// Marca el campo como sensible
        /// </summary>
        public void MarcarComoSensible()
        {
            EsCampoSensible = true;

            // Ocultar valores si es sensible
            if (!string.IsNullOrEmpty(ValorAnterior))
                ValorAnteriorFormateado = "***";

            if (!string.IsNullOrEmpty(ValorNuevo))
                ValorNuevoFormateado = "***";
        }

        /// <summary>
        /// Establece información del usuario
        /// </summary>
        public void EstablecerUsuario(int? usuarioId, string nombreUsuario)
        {
            UsuarioId = usuarioId;
            NombreUsuario = nombreUsuario;
        }

        /// <summary>
        /// Asocia con un log de auditoría
        /// </summary>
        public void AsociarLogAuditoria(int logAuditoriaId)
        {
            LogAuditoriaId = logAuditoriaId;
        }

        /// <summary>
        /// Compara valores para determinar el tipo de cambio
        /// </summary>
        public string ObtenerTipoCambio()
        {
            if (AmbosNulos) return "Sin cambio";
            if (EsNuloANoNulo) return "Nuevo valor";
            if (EsNoNuloANulo) return "Valor eliminado";
            if (ValorCambio) return "Valor modificado";
            return "Sin cambio";
        }

        /// <summary>
        /// Formatea el valor según el tipo de dato
        /// </summary>
        public string FormatearValor(string valor)
        {
            if (string.IsNullOrEmpty(valor)) return "(vacío)";
            if (EsCampoSensible) return "***";

            // Si es fecha
            if (EsCampoFecha && DateTime.TryParse(valor, out var fecha))
                return fecha.ToString("dd/MM/yyyy HH:mm");

            // Si es booleano
            if (EsCampoBooleano && bool.TryParse(valor, out var booleano))
                return booleano ? "Sí" : "No";

            // Si es numérico con decimales
            if (EsCampoNumerico && decimal.TryParse(valor, out var numero))
            {
                if (TipoDato.ToLower().Contains("decimal") || TipoDato.ToLower().Contains("double"))
                    return numero.ToString("N2");
                return numero.ToString("N0");
            }

            return valor;
        }

        /// <summary>
        /// Obtiene un resumen del cambio
        /// </summary>
        public string ObtenerResumen()
        {
            var resumen = $"[{FechaCambio:dd/MM/yyyy HH:mm}] ";
            resumen += $"{NombreEntidad} #{EntidadId} - ";
            resumen += DescripcionCambio;

            if (!string.IsNullOrEmpty(NombreUsuario))
                resumen += $" por {NombreUsuario}";

            return resumen;
        }

        /// <summary>
        /// Valida que el cambio sea correcto
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(NombreEntidad))
                errores.Add("El nombre de la entidad es requerido");

            if (EntidadId <= 0)
                errores.Add("El ID de la entidad debe ser mayor a cero");

            if (string.IsNullOrWhiteSpace(NombreCampo))
                errores.Add("El nombre del campo es requerido");

            if (string.IsNullOrWhiteSpace(TipoDato))
                errores.Add("El tipo de dato es requerido");

            if (FechaCambio > DateTime.Now)
                errores.Add("La fecha del cambio no puede ser futura");

            if (ValorAnterior == ValorNuevo && !AmbosNulos)
                errores.Add("El valor anterior y el nuevo son iguales");

            return errores;
        }

        #endregion

        #region Métodos Estáticos de Creación

        /// <summary>
        /// Crea un registro de cambio
        /// </summary>
        public static CambioEntidad CrearCambio(int? escuelaId, string nombreEntidad, int entidadId,
                                               string nombreCampo, string tipoDato, string valorAnterior,
                                               string valorNuevo, int? usuarioId = null, string nombreUsuario = null,
                                               int? logAuditoriaId = null)
        {
            var cambio = new CambioEntidad
            {
                EscuelaId = escuelaId,
                NombreEntidad = nombreEntidad,
                EntidadId = entidadId,
                NombreCampo = nombreCampo,
                TipoDato = tipoDato,
                ValorAnterior = valorAnterior,
                ValorNuevo = valorNuevo,
                UsuarioId = usuarioId,
                NombreUsuario = nombreUsuario,
                LogAuditoriaId = logAuditoriaId
            };

            // Formatear valores automáticamente
            cambio.ValorAnteriorFormateado = cambio.FormatearValor(valorAnterior);
            cambio.ValorNuevoFormateado = cambio.FormatearValor(valorNuevo);

            return cambio;
        }

        /// <summary>
        /// Crea múltiples cambios a partir de un diccionario
        /// </summary>
        public static List<CambioEntidad> CrearCambiosDesdeDict(int? escuelaId, string nombreEntidad, int entidadId,
                                                                Dictionary<string, (string valorAnterior, string valorNuevo, string tipoDato)> cambios,
                                                                int? usuarioId = null, string? nombreUsuario = null,
                                                                int? logAuditoriaId = null)
        {
            var listaCambios = new List<CambioEntidad>();

            foreach (var cambio in cambios)
            {
                listaCambios.Add(CrearCambio(
                    escuelaId,
                    nombreEntidad,
                    entidadId,
                    cambio.Key,
                    cambio.Value.tipoDato,
                    cambio.Value.valorAnterior,
                    cambio.Value.valorNuevo,
                    usuarioId,
                    nombreUsuario,
                    logAuditoriaId
                ));
            }

            return listaCambios;
        }

        #endregion
    }
}