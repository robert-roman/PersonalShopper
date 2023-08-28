using Microsoft.AspNetCore.Identity;

namespace PersonalShopper.DAL.Models
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
