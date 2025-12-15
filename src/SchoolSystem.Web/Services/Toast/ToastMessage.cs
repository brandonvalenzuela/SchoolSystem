namespace SchoolSystem.Web.Services.Toast
{
    public class ToastMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Message { get; set; }
        public ToastLevel Level { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
