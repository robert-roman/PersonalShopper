using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;

namespace PersonalShopper.Services.OrderService
{
    public interface IOrderService
    {
        Task<ICollection<OrderProduct>?> CreateOrderProducts(Order order);
    }
}
