using PersonalShopper.DAL.Models;
using PersonalShopper.DAL;
using PersonalShopper.DAL.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace PersonalShopper.DAL.Repositories.SessionTokenRepository
{
    public class SessionTokenRepository : GenericRepository<SessionToken>, ISessionTokenRepository
    {
        public SessionTokenRepository(ApplicationDbContext context) : base(context) { }

        public async Task<SessionToken> GetByJTI(string jti)
        {
            return await _context.SessionTokens
                .FirstOrDefaultAsync(t => t.Jti.Equals(jti));
        }
    }
}
