namespace BiiGBackend.Models.Responses
{
	public class ShoppingCartItemResponse
	{
		public Guid Id { get; set; }
		public string ProductName { get; set; }
		public string ProductBrand { get; set; }
		public double ProductDisplayPrice { get; set; }
		public bool IsDiscounted { get; set; }
		public double ProductRealPrice { get; set; }
		public string ProductImage { get; set; }
		public int ProductAmount { get; set; }
		public string Size { get; set; } = string.Empty;

	}
}

/*
 ShoppingCartItemResponse{
    productName: string
    productBrand: string
    productDisplayPrice: string
    isDiscounted: boolean
    productRealPrice: string
    productImage: string
    productUrl: string
    productAmount:number
}
 
 */