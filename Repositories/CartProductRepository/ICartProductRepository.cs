using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.CartProductRepository
{
    public interface ICartProductRepository : IGenericRepository<CartProduct>
    {
        Task<List<CartProduct>> GetProductsByCartId (int cartId);
    }
}
