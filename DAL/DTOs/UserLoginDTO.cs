using System.ComponentModel.DataAnnotations;

namespace PersonalShopper.DAL.DTOs
{
    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
