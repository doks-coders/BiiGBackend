namespace BiiGBackend.Models.Requests
{

	public class UpdateProductPropRequest
	{
		public Guid ProductId { get; set; }
		public string Field { get; set; }
		public bool Value { get; set; }
	}
}
