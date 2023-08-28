using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalShopper.DAL.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public ICollection<CartProduct>? CartProducts { get; set; }
        public float CartPrice { get; set; } 
    }
}
