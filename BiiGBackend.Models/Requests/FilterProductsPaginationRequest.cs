namespace BiiGBackend.Models.Requests
{
    public class FilterProductsPaginationRequest : PaginationRequest
    {
        public string Category { get; set; } = string.Empty;
        public string Brands { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
