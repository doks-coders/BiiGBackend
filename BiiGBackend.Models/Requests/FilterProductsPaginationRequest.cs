namespace BiiGBackend.Models.Requests
{
	public class FilterProductsPaginationRequest : PaginationRequest
	{
		public string Category { get; set; } = "";
		public string Brands { get; set; } = "";
		public string Size { get; set; } = "";
	}
}
