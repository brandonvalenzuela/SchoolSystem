namespace SchoolSystem.Web.Services
{
    public class UiService
    {
        // Propiedad para guardar el mensaje temporalmente
        private string? _successMessage;

        // Método para guardar el mensaje (lo usa el Formulario antes de irse)
        public void ShowSuccess(string message)
        {
            _successMessage = message;
        }

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
