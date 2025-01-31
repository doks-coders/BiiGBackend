namespace BiiGBackend.Models.Responses
{
    /**
     * export interface OrderExtendedResponse {
    orderItems:OrderItemResponse []
    orderStatus:string,
    paymentStatus:string,    	
}
     */
    public class OrderExtendedResponse
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StreetAddress { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public List<OrderItemResponse> OrderItems { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalInNaira { get; set; }
        public double? TotalInDollars { get; set; }
        public double? USDToNairaRate { get; set; }
        public double? LogisticsFee { get; set; }
    }
    public class OrderItemResponse
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string ProductBrand { get; set; }
        public string ProductDisplayPrice { get; set; }
        public string ProductRealPrice { get; set; }
        public string ProductImage { get; set; }
        public bool IsDiscounted { get; set; }
        public int ProductAmount { get; set; }
    }
}
/*
 * export interface OrderExtendedResponse {
    orderItems:OrderItemResponse []
    orderStatus:string,
    paymentStatus:string,    	
}
export interface OrderItemResponse{
    id:string
    productName: string
    size:string,
    productBrand: string
    productDisplayPrice: string
    isDiscounted: boolean
    productRealPrice: string
    productImage: string
    productUrl: string
    productAmount:number,
}
 */