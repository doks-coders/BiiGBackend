namespace BiiGBackend.Models.Requests
{
    public class ProductRequest
    {
        public string ProductName { get; set; }
        public Guid ProductBrandId { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductDescription { get; set; }
        public double ProductRealPrice { get; set; }
        public double ProductDiscountPercent { get; set; } = 0;
        public int ProductStockAmount { get; set; } = 1;
        public string ProductSizes { get; set; } = string.Empty;
        public bool IsDiscounted { get; set; } = false;
    }

}
