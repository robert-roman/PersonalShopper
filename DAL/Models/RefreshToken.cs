using System.ComponentModel.DataAnnotations;

namespace PersonalShopper.DAL.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }

        public User User { get; set; }
    }
}
