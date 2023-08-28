using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.ProductRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetProductByName(string name);
        Task<List<Product>> GetProductsWithCategory(string name);
        Task<List<Product>> GetProductsContainingDescription(string name);

    }
}
