using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.DTOs.UserDTOs;
using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.RoleDTOs;

namespace Study1CApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Study1cDbContext _context;

        public UserRepository(Study1cDbContext context) => _context = context;

        public async Task<ICollection<UserDTO>> GetAllUsers()
        {
            var users = await _context.Users.Select(x => new UserDTO()
            {
                UserId = x.UserId,
                UserSurname = x.UserSurname,
                UserName = x.UserName,
                UserPatronymic = x.UserPatronymic,
                IsFirst = x.IsFirst
            }).ToListAsync();

            return users;
        }

        public async Task UpdateUser()
        {

        }
    }
}
