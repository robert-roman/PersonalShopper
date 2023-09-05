using PersonalShopper.DAL.Models;

namespace PersonalShopper.DAL.DTOs
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public CartDTO? Cart { get; set; }
        public virtual ICollection<OrderDTO> UserOrders { get; set; }

        public UserProfileDTO(User user)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Age = user.Age;
            Cart = new CartDTO(user.Cart);
            UserOrders = user.UserOrders.Select( uo => new OrderDTO(uo)).ToList();
        }
    }
}
