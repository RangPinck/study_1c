using Study1CApi.DTOs.UserDTOs;

namespace Study1CApi.Interfaces
{
    public interface IUserRepository
    {
        public Task<ICollection<UserDTO>> GetAllUsers();
        
        public Task UpdateUser();
    }
}
