using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public ICollection<CartProductDTO> CartProducts { get; set; }
        public float CartPrice { get; set; }

        public CartDTO(Cart cart)
        {
            CartId = cart.CartId;
            UserId = cart.UserId;
            CartProducts = cart.CartProducts.Select(cp => new CartProductDTO(cp)).ToList();
            CartPrice = cart.CartPrice;
        }
    }
}
