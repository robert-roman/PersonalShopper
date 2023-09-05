using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;

namespace PersonalShopper.Services.CartProductService
{
    public interface ICartService
    {
        //Task ClearCart(Cart boughtCart);
        Task CalculateCartPrice(Cart cart);
    }
}
