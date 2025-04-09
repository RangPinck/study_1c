using Study1CApi.DTOs.UserDTOs;

namespace Study1CApi.Interfaces
{
    public interface IUserRepository
    {
        public Task<ICollection<UserDTO>> GetAllUsers(bool isAdmin = false);

        public Task<IEnumerable<CourseAuthorDTO>> GetAuthorsForCourses();
        
        public Task<bool> UserIsExist(Guid userId);
    }
}
