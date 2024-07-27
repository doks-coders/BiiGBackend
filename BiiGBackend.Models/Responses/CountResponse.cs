namespace BiiGBackend.Models.Responses
{
	public class CountResponse
	{
		public Guid Id { get; set; }
		public int Count { get; set; }
		public double Price { get; set; }
		public double RealPrice { get; set; }
	}
}
