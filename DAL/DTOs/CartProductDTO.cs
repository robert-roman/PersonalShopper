using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class CartProductDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int CartProductQuantity { get; set; }

        public CartProductDTO() { }

        public CartProductDTO(CartProduct cartProduct)
        {
            UserId = cartProduct.CartId;
            ProductId = cartProduct.ProductId;
            CartProductQuantity = cartProduct.CartProductQuantity;
        }
    }
}
