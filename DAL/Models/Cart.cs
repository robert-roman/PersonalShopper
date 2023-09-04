using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalShopper.DAL.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<CartProduct> CartProducts { get; set; }
        public float CartPrice { get; set; }
    }
}