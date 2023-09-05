using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalShopper.Controllers;
using PersonalShopper.DAL;
using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.CartProductRepository
{
    public class CartProductRepository : GenericRepository<CartProduct>, ICartProductRepository
    {
        public CartProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ICollection<CartProduct>> GetProductsByCartId(int cartId) =>
            await _context.CartProducts.Include(x => x.Cart).Include(x => x.Product).Where(cp => cp.CartId.Equals(cartId)).ToListAsync();
    }
}
