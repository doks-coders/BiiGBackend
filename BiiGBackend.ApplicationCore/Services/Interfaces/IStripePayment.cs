using BiiGBackend.Models.Entities.Orders;

namespace BiiGBackend.ApplicationCore.Services.Interfaces
{
    public interface IStripePayment
    {
        Task<string> InitialisePayment(Guid OrderHeaderId, IEnumerable<OrderItem> orderItems);
    }
}
