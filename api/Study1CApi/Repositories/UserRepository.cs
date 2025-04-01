using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Study1CApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Study1cDbContext _context;

        public UserRepository(Study1cDbContext context) => _context = context;

        public async Task<ICollection<UserDTO>> GetAllUsers()
        {
            var users = await _context.Users.Include(x => x.UserRoleNavigation).Select(x => new UserDTO()
            {
                UserId = x.UserId,
                UserLogin = x.UserLogin,
                UserSurname = x.UserSurname,
                UserName = x.UserName,
                UserPatronymic = x.UserPatronymic,
                UserDataCreate = x.UserDataCreate,
                IsFirst = x.IsFirst,
                UserRole = new RoleDTO()
                {
                    RoleId = x.UserRoleNavigation.RoleId,
                    RoleName = x.UserRoleNavigation.RoleName
                }
            }).ToListAsync();

            return users;
        }
    }
}
