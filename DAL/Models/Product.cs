using System.ComponentModel.DataAnnotations;

namespace PersonalShopper.DAL.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductBrand { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice { get; set; }
        public int? ProductStock { get; set; }
        public virtual ICollection<CartProduct>? CartProducts { get; set; }
    }
}
