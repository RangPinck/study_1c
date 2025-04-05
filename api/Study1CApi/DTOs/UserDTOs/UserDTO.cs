using Study1CApi.DTOs.RoleDTOs;

namespace Study1CApi.DTOs.UserDTOs
{
    public class UserDTO
    {
        public Guid UserId { get; set; }

        public string UserLogin { get; set; } = null!;

        public string UserHashPassword { get; set; } = null!;

        public string UserSurname { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? UserPatronymic { get; set; }

        public RoleDTO UserRole { get; set; } = new RoleDTO();

        public bool IsFirst { get; set; }
    }
}
