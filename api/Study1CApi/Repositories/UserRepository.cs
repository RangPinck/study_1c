using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.DTOs.UserDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Study1CApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Study1cDbContext _context;
        private readonly UserManager<AuthUser> _userManager;

        public UserRepository(Study1cDbContext context, UserManager<AuthUser> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ICollection<UserDTO>> GetAllUsers(bool isAdmin = false)
        {
            try
            {
                var profiles = await _context.Users.AsNoTracking().ToListAsync();
                var users = await _userManager.Users.AsNoTracking().ToListAsync();
                List<UserDTO> result = new List<UserDTO>();
                if (isAdmin)
                {
                    result = users.Select(user => new UserDTO()
                    {
                        UserId = user.Id,
                        UserLogin = user.Email,
                        UserSurname = profiles.FirstOrDefault(x => x.UserId == user.Id).UserSurname,
                        UserName = profiles.FirstOrDefault(x => x.UserId == user.Id).UserName,
                        UserPatronymic = profiles.FirstOrDefault(x => x.UserId == user.Id).UserPatronymic,
                        UserDataCreate = user.UserDataCreate,
                        UserRole = _userManager.GetRolesAsync(user).Result.ToList(),
                    }).ToList();
                }
                else
                {
                    result = users.Select(user => new UserDTO()
                    {
                        UserId = user.Id,
                        UserLogin = user.Email,
                        UserSurname = profiles.FirstOrDefault(x => x.UserId == user.Id).UserSurname,
                        UserName = profiles.FirstOrDefault(x => x.UserId == user.Id).UserName,
                        UserPatronymic = profiles.FirstOrDefault(x => x.UserId == user.Id).UserPatronymic,
                        UserDataCreate = user.UserDataCreate,
                        UserRole = _userManager.GetRolesAsync(user).Result.ToList(),
                    }).Where(x => x.UserRole.Contains("Ученик")).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<UserDTO>();
            }
        }
    }
}
