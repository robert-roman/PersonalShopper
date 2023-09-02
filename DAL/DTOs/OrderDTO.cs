using PersonalShopper.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalShopper.DAL.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public Cart OrderCart { get; set; }
        public string OrderStatus { get; set; }

        public OrderDTO(Order order)
        {
            OrderId = order.OrderId;
            UserId = order.UserId;
            OrderCart = order.OrderCart;
            OrderStatus = order.OrderStatus;
        }
    }
}
