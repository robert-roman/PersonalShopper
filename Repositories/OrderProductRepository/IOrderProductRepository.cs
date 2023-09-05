using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.OrderProductRepository
{
    public interface IOrderProductRepository : IGenericRepository<OrderProduct>
    {
        Task<List<(int ProductId, string ProductName, float ProductPrice, float OrderProductPrice, int OrderProductQuantity)>> 
                   ComparePricesForAProduct(); 
    }
}
