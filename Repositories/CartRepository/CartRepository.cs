using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalShopper.DAL;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.CartRepository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }


        public async Task<Cart> GetCartById(int id) =>
            await _context.Carts.Include(x=> x.CartProducts).ThenInclude( y => y.Product).Where(cart => cart.CartId.Equals(id)).FirstOrDefaultAsync();
        public async Task<ActionResult<Cart>> GetProductFromCartByName(string productName)
        {
            try
            {
                var dbContext = (ApplicationDbContext) _context;

                var cart = await dbContext.Carts
                    .Include(c => c.CartProducts)
                    .ThenInclude(cp => cp.Product)
                    .Where(c => c.CartProducts.Any(cp => cp.Product.ProductName == productName))
                    .FirstOrDefaultAsync();

                if (cart != null)
                {
                    return cart;
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
