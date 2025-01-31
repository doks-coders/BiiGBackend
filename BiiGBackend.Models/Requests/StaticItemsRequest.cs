namespace BiiGBackend.Models.Requests
{

    public class CategoryRequest()
    {
        public Guid? Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class BrandRequest()
    {
        public Guid? Id { get; set; }
        public string BrandName { get; set; }
    }
}
