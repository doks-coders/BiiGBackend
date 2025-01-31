namespace BiiGBackend.Models.Entities.Product
{
    public class Photo : BaseEntity
    {
        public bool IsMain { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public Guid ProductId { get; set; }
        public ProductItem Product { get; set; }
    }
}
