using Study1CApi.DTOs;

namespace Study1CApi.Interfaces
{
    public interface IUserRepository
    {
        public Task<ICollection<UserDTO>> GetAllUsers();
    }
}
