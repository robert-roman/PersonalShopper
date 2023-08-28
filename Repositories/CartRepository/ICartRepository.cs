using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.CartRepository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        /*Task<Cart> SortCartProductsByPrice();*/
        Task<ActionResult<Cart>> GetProductFromCartByName(string productName);

    }
}
