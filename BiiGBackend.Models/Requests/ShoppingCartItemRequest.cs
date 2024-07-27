namespace BiiGBackend.Models.Requests
{
	public class ShoppingCartItemRequest
	{
		public Guid ProductId { get; set; }

		public int Count { get; set; }

		public string Size { get; set; }

	}
}
