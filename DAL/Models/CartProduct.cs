using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonalShopper.DAL.Models
{
    public class CartProduct
    {
        [Key, Column(Order=0)]
        public int CartId { get; set; }
        [Key, Column(Order=1)]
        public int ProductId { get; set; }
        public int CartProductQuantity { get; set; }
        [ForeignKey("ProductId"), JsonIgnore]
        public Product? Product { get; set; }
        [ForeignKey("CartId"), JsonIgnore]
        public Cart? Cart { get; set; }
    }
}
