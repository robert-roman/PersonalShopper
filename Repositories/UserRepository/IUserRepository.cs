﻿using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.UserRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserAndUserRoleById(int userId);
        Task<User> GetUserByEmail(string email);
    }
}
