using Microsoft.EntityFrameworkCore;
using PersonalShopper.Controllers;
using PersonalShopper.DAL;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.CartProductRepository
{
    public class CartProductRepository : GenericRepository<CartProduct>, ICartProductRepository
    {
        public CartProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<CartProduct>> GetProductsByCartId(int cartId) =>
            await _context.CartProducts.Where(cp => cp.UserId.Equals(cartId)).ToListAsync();
    }
}
