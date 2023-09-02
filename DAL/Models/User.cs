using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalShopper.DAL.Models
{
    public class User : IdentityUser<int>
    {
        public User() : base() { }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Order> UserOrders { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
