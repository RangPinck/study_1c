using Study1CApi.DTOs.UserDTOs;

namespace Study1CApi.Interfaces
{
    public interface IUserRepository
    {
        public Task<ICollection<UserDTO>> GetAllUsersAsync(bool isAdmin = false);

        public Task<IEnumerable<CourseAuthorDTO>> GetAuthorsForCoursesAsync();
        
        public Task<bool> UserIsExistAsync(Guid userId);
    }
}
