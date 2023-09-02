using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalShopper.DAL.Models
{
    public class Cart
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public ICollection<CartProduct> CartProducts { get; set; }
        public float CartPrice { get; set; }
    }
}