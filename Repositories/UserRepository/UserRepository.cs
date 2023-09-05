using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;
using PersonalShopper.DAL;
using Microsoft.EntityFrameworkCore;

namespace PersonalShopper.Repositories.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<User>> GetAllUsersAsync() =>
        await _context.Users.Include(x => x.Cart).ThenInclude(y => y.CartProducts).ThenInclude(z => z.Product)
                            .Include(x => x.UserOrders)
                            .Include(x => x.UserRoles).ThenInclude(y => y.Role)
                            .ToListAsync();
        public async Task<User> GetUserById(int userId) =>
            await _context.Users.Include(x => x.Cart).ThenInclude(y => y.CartProducts).ThenInclude(z => z.Product)
                                .Include(x => x.UserOrders)
                                .Include(x => x.UserRoles).ThenInclude(y => y.Role)
                                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

        public async Task<User> GetUserByEmail(string email) =>
            await _context.Users.Include(x => x.Cart).ThenInclude(y => y.CartProducts).ThenInclude(z => z.Product)
                                .Include(x => x.UserOrders)
                                .Include(x => x.UserRoles).ThenInclude(y => y.Role)
                                .Where(user => user.Email.Equals(email)).FirstOrDefaultAsync();
    }
}
