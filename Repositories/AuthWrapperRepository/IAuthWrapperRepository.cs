using PersonalShopper.DAL.Repositories.SessionTokenRepository;
using PersonalShopper.Repositories.UserRepository;

namespace PersonalShopper.DAL.Repositories.AuthWrapperRepository
{
    public interface IAuthWrapperRepository
    {
        ISessionTokenRepository SessionToken { get; }
        IUserRepository User { get; }
        IRefreshTokenRepository RefreshToken { get; }

        Task SaveAsync();
    }
}
