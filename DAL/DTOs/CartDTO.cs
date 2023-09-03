using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class CartDTO
    {
        public int UserId { get; set; }
        public ICollection<CartProduct>? CartProducts { get; set; }
        public float CartPrice { get; set; }

        public CartDTO(Cart cartDTO)
        {
            UserId = cartDTO.UserId;
            CartProducts = cartDTO.CartProducts;
            CartPrice = cartDTO.CartPrice;
        }
    }
}
