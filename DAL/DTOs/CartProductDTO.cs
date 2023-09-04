using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class CartProductDTO
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public int CartProductQuantity { get; set; }

        public CartProductDTO() { }

        public CartProductDTO(CartProduct cartProduct)
        {
            CartId = cartProduct.CartId;
            ProductId = cartProduct.ProductId;
            ProductName = cartProduct.Product.ProductName;
            ProductPrice = cartProduct.Product.ProductPrice;
            CartProductQuantity = cartProduct.CartProductQuantity;
        }
    }
}
