using PersonalShopper.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalShopper.DAL.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
        public string OrderStatus { get; set; }
        public string OrderDate { get; set; }
        public float OrderPrice { get; set; }

        public OrderDTO(Order order)
        {
            OrderId = order.OrderId;
            UserId = order.UserId;
            OrderProducts = order.OrderProducts;
            OrderStatus = order.OrderStatus;
            //OrderDate = DateOnly.FromDateTime(order.OrderPlaceDate);
            OrderDate = order.OrderPlaceDate.ToString("dd-MM-yyyy");
            OrderPrice = order.OrderPrice;
        }
    }
}
