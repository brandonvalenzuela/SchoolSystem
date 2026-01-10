namespace SchoolSystem.Web.Services
{
    public class UiService
    {
        // Propiedad para guardar el mensaje temporalmente
        private string? _successMessage;

        // Método para obtener y limpiar el mensaje (lo usa la Lista al llegar)
        public string GetSuccessMessage()
        {
            var msg = _successMessage;
            _successMessage = null; // Limpiamos para que no salga al recargar
            return msg;
        }

        // Propiedad para verificar si hay mensaje pendiente
        public bool HasMessage => !string.IsNullOrEmpty(_successMessage);
    }
}
