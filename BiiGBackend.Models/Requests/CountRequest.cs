namespace BiiGBackend.Models.Requests
{
	public class CountRequest
	{
		public Guid ShoppingCartId { get; set; }
		public string Mode { get; set; }
	}
}
