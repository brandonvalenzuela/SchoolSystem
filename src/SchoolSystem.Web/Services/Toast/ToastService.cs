namespace SchoolSystem.Web.Services.Toast
{
    public class ToastService : IDisposable
    {
        // Evento para avisar al componente que hay cambios
        public event Action OnChange;

        public List<ToastMessage> Toasts { get; private set; } = new List<ToastMessage>();


        // Helpers rápidos
        public void ShowSuccess(string message) => ShowToast(message, ToastLevel.Success);
        public void ShowError(string message) => ShowToast(message, ToastLevel.Error);
        public void ShowInfo(string message) => ShowToast(message, ToastLevel.Info);
        public void ShowWarning(string message) => ShowToast(message, ToastLevel.Warning);

        // Método principal para mostrar mensajes
        public void ShowToast(string message, ToastLevel level)
        {
            var toast = new ToastMessage
            {
                Message = message,
                Level = level,
                Title = GetTitleForLevel(level)
            };

            Toasts.Add(toast);
            NotifyStateChanged();

            // Configurar autolimpieza (5 segundos)
            StartTimer(toast, 5000);
        }

        // Método para eliminar manualmente (clic en la X)
        public void Remove(ToastMessage toast)
        {
            if (!Toasts.Contains(toast))
            {
                return;
            }
            Toasts.Remove(toast);
            NotifyStateChanged();
        }

        private async void StartTimer(ToastMessage toast, int delay)
        {
            await Task.Delay(delay);
            Remove(toast);
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        private string GetTitleForLevel(ToastLevel level) => level switch
        {
            ToastLevel.Success => "Éxito",
            ToastLevel.Error => "Error",
            ToastLevel.Warning => "Advertencia",
            _ => "Información"
        };

        public void Dispose()
        {
            // Limpieza si fuera necesaria
        }
    }
}
