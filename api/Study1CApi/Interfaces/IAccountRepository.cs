using Study1CApi.DTOs.AccountDTOs;
using Study1CApi.DTOs.UserDTOs;

namespace Study1CApi.Interfaces
{
    public interface IAccountRepository
    {
        public Task<bool> UpdateUserProfile(UpdateProfileDTO updateProfile);

        public Task<bool> SaveChangesAsync();

        public Task<UserDTO> GetUserDataByIdAsync(Guid userId);

        public Task<bool> RegistrationUserFirstLogin(Guid userId);
    }
}
