using Study1CApi.DTOs.UserDTOs;

namespace Study1CApi.Interfaces
{
    public interface IUserRepository
    {
        public Task<ICollection<UserDTO>> GetAllUsers();

        // public Task<UserDTO> GetUserById(Guid userId);

        // public Task<bool> AddUser(UserDTO newUser);

        // public Task<bool> UpdateUser(UserDTO upUser);

        // public Task<bool> DeleteUser(Guid userId);

        // public Task<bool> UserIsExists(UserDTO upUser);
    }
}
