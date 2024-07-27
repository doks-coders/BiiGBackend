namespace BiiGBackend.Models.Responses
{
	public class OrderResponse
	{
		public Guid Id { get; set; }
		public string EmailAddress { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string TrackingNumber { get; set; }
		public double TotalPrice { get; set; }
		public DateTime DateCreated { get; set; }
	}
}

