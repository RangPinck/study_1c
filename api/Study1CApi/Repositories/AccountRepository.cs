using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.AccountDTOs;
using Study1CApi.DTOs.UserDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace Study1CApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Study1cDbContext _context;

        public AccountRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> GetUserDataByIdAsync(Guid userId)
        {
            var user = await _context.Users.AsNoTracking().FirstAsync(x => x.UserId == userId);
            return new UserDTO()
            {
                UserId = userId,
                UserName = user.UserName,
                UserSurname = user.UserName,
                UserPatronymic = user.UserName,
                IsFirst = user.IsFirst
            };
        }

        public async Task<bool> RegistrationUserFirstLogin(Guid userId)
        {
            var profile = await _context.Users.FirstAsync(x => x.UserId == userId);

            profile.IsFirst = false;

            _context.Users.Update(profile);

            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<bool> UpdateUserProfile(UpdateProfileDTO updateProfile)
        {

            var profile = await _context.Users.FirstAsync(x => x.UserId == updateProfile.UserId);

            profile.UserName = updateProfile.UserName;
            profile.UserSurname = updateProfile.UserSurname;
            profile.UserPatronymic = updateProfile.UserPatronymic;

            _context.Users.Update(profile);

            return await SaveChangesAsync();
        }
    }
}
