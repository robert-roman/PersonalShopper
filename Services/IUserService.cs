using PersonalShopper.DAL.DTOs;

namespace PersonalShopper.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegisterDTO dto);
        Task<string> LoginUser(UserLoginDTO dto);
    }
}
