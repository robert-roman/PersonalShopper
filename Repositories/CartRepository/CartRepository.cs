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
            await _context.Carts.Where(cart => cart.CartId.Equals(id)).FirstOrDefaultAsync();
        public async Task<ActionResult<Cart>> GetProductFromCartByName(string productName)
        {
            try
            {
                var dbContext = (ApplicationDbContext) _context;

                // Search for the product in the cart by name
                var cart = await dbContext.Carts
                    .Include(c => c.CartProducts)
                    .ThenInclude(cp => cp.Product)
                    .Where(c => c.CartProducts.Any(cp => cp.Product.ProductName == productName)) // Change to ProductName
                    .FirstOrDefaultAsync();

                if (cart != null)
                {
                    return cart;
                }
                else
                {
                    // If the cart or the product is not found, return a not found result
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                // You can log the exception or return an appropriate response
                // For simplicity, rethrowing the exception here
                throw ex;
            }
        }
    }
}
