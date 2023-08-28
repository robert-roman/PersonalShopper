using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.OrderRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
    }
}
