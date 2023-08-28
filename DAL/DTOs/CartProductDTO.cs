using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class CartProductDTO
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int CartProductQuantity { get; set; }

        public CartProductDTO(CartProduct cartProduct)
        {
            CartId = cartProduct.CartId;
            ProductId = cartProduct.ProductId;
            CartProductQuantity = cartProduct.CartProductQuantity;
        }
    }
}
