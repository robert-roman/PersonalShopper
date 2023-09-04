using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }

        public string ProductBrand { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice { get; set; }
        public int ProductStock { get; set; }

        public ProductDTO() { }
        public ProductDTO(Product product)
        {
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            ProductCategory = product.ProductCategory;
            ProductBrand = product.ProductBrand;
            ProductDescription = product.ProductDescription;
            ProductPrice = product.ProductPrice;
            ProductStock = product.ProductStock;
        }
    }
}
