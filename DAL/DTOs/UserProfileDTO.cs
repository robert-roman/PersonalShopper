using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Cart? Cart { get; set; }
        public virtual ICollection<Order> UserOrders { get; set; }

        public UserProfileDTO(User user)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Age = user.Age;
            Cart = user.Cart;
            UserOrders = user.UserOrders;
        }
    }
}
