using PersonalShopper.DAL.Repositories.SessionTokenRepository;
using PersonalShopper.Repositories.UserRepository;

namespace PersonalShopper.DAL.Repositories.AuthWrapperRepository
{
    public class AuthWrapperRepository : IAuthWrapperRepository
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository _user;
        private ISessionTokenRepository _sessionToken;
        private IRefreshTokenRepository _refreshToken;

        public AuthWrapperRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository User
        {
            get
            {
                if (_user == null) _user = new UserRepository(_context);
                return _user;
            }
        }

        public ISessionTokenRepository SessionToken
        {
            get
            {
                if (_sessionToken == null) _sessionToken = new SessionTokenRepository.SessionTokenRepository(_context);
                return _sessionToken;
            }
        }

        public IRefreshTokenRepository RefreshToken
        {
            get
            {
                if (_refreshToken == null) _refreshToken = new RefreshTokenRepository(_context);
                return _refreshToken;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
