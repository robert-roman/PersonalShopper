using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class CartDTO
    {
        public int UserID { get; set; }
        public ICollection<Product> CartProducts { get; set; }
        public float CartPrice { get; set; }

        public CartDTO(CartDTO cartDTO)
        {
            UserID = cartDTO.UserID;
            CartProducts = cartDTO.CartProducts;
            CartPrice = cartDTO.CartPrice;
        }
    }
}
