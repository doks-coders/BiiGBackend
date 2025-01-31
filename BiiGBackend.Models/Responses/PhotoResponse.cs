namespace BiiGBackend.Models.Responses
{
    public class PhotoResponse
    {
        public Guid Id { get; set; }
        public bool IsMain { get; set; }
        public string Url { get; set; }
        public Guid ProductId { get; set; }
    }
}
