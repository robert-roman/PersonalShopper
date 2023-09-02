using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalShopper.DAL.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public Cart OrderCart { get; set; }
        public string OrderStatus { get; set; }
        //an OrderStatus can be Placed, OnTrack, Delivered

    }
}
