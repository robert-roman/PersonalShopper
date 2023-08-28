using Microsoft.AspNetCore.Identity;

namespace PersonalShopper.DAL.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        public Role Role { get; set; }
        public User User { get; set; }
    }
}
