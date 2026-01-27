namespace SchoolSystem.Application.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando hay un conflicto de concurrencia
    /// (ej: violación de índice UNIQUE por actualización simultánea).
    /// Se mapea a 409 Conflict en la API.
    /// </summary>
    public class ConflictException : ApplicationException
    {
        public ConflictException(string message) : base(message) { }
        public ConflictException(string message, Exception inner) : base(message, inner) { }
    }
}
