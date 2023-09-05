using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalShopper.DAL;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.OrderRepository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Order> GetOrderById(int id) =>
            await _context.Orders.Include(x => x.OrderCart).ThenInclude(y => y.CartProducts).ThenInclude(z => z.Product)
                                 .Where(order => order.OrderId.Equals(id)).FirstOrDefaultAsync();
       
        public async Task<List<Order>> GetAllUserOrders(int id) =>
            await _context.Orders.Include(x => x.OrderCart).ThenInclude(y => y.CartProducts).ThenInclude(z => z.Product)
                                 .Where(order => order.UserId.Equals(id)).ToListAsync();
       
        public async Task<List<Order>> GetOrdersByStatus(string status) =>
            await _context.Orders.Include(x => x.OrderCart).ThenInclude(y => y.CartProducts).ThenInclude(z => z.Product)
                                 .Where(order => order.OrderStatus.Equals(status)).ToListAsync();

    }
}
