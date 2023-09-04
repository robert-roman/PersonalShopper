using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public ICollection<CartProduct> CartProducts { get; set; }
        public float CartPrice { get; set; }

        public CartDTO(Cart cart)
        {
            CartId = cart.CartId;
            UserId = cart.UserId;
            CartProducts = cart.CartProducts;
            CartPrice = cart.CartPrice;
        }
    }
}
