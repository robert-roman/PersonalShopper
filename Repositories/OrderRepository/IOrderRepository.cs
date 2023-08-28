using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.OrderRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetOrderById(int id);
        Task<List<Order>> GetAllUserOrders(int id);
        Task<List<Order>> GetOrdersByStatus(string status);
    }
}
