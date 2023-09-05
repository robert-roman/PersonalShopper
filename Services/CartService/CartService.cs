using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using PersonalShopper.Repositories.UnitOfWork;

namespace PersonalShopper.Services.CartProductService
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public async Task ClearCart(Cart boughtCart)
        { 
            foreach (CartProduct boughtProduct in boughtCart.CartProducts)
            {
                await _unitOfWork.CartProducts.Delete(boughtProduct);
            }
            boughtCart.CartPrice = 0;
            boughtCart.CartProducts.Clear();
            _unitOfWork.Save();
        }

        public async Task<float> CalculateCartPrice(Cart cart)
        {
            return 0;
        }
    }
}
