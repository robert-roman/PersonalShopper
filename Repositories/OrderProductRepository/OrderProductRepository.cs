using PersonalShopper.DAL;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.OrderProductRepository
{
    public class OrderProductRepository : GenericRepository<OrderProduct>,IOrderProductRepository
    {
        public OrderProductRepository(ApplicationDbContext context) : base(context) { }
    }
}
