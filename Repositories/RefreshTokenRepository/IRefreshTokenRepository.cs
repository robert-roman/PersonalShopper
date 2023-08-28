using PersonalShopper.DAL.Models;
using System.Threading.Tasks;

namespace PersonalShopper.DAL.Repositories.AuthWrapperRepository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetRefreshToken(string token);
        Task CreateRefreshToken(int userId, RefreshToken refreshToken);

        Task UpdateRefreshToken(RefreshToken refreshToken);
        Task DeleteRefreshToken(RefreshToken refreshToken);
    }
}
