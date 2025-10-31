namespace SchoolSystem.Domain.Enums.Auditoria
{
    /// <summary>
    /// Tipos de acciones auditables en el sistema
    /// </summary>
    public enum TipoAccion
    {
        Crear = 1,
        Actualizar = 2,
        Eliminar = 3,
        Leer = 4,
        Login = 5,
        Logout = 6,
        CambioPassword = 7,
        RecuperarPassword = 8,
        Exportar = 9,
        Importar = 10,
        Descargar = 11,
        Imprimir = 12,
        Enviar = 13,
        Aprobar = 14,
        Rechazar = 15,
        Cancelar = 16,
        Restaurar = 17,
        Archivar = 18,
        Compartir = 19,
        Configurar = 20,
        Otro = 99
    }
}