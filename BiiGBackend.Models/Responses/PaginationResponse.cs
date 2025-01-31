namespace BiiGBackend.Models.Responses
{
    public class PaginationResponse
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public object Items { get; set; }
    }
}
