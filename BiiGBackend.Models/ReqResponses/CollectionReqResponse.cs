namespace BiiGBackend.Models.ReqResponses
{
	public class CollectionReqResponse
	{
		public Guid? Id { get; set; }
		public string CollectionName { get; set; }
		public string CollectionCaption { get; set; }
		public string CollectionImageUrl { get; set; }
		public string CollectionUrl { get; set; }
	}
}
