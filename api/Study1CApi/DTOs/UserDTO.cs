using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study1CApi.DTOs
{
    public class UserDTO
    {
        public Guid UserId { get; set; }

        public string UserLogin { get; set; } = null!;

        public string UserHashPassword { get; set; } = null!;

        public string UserSurname { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? UserPatronymic { get; set; }

        public DateTime UserDataCreate { get; set; }

        public RoleDTO UserRole { get; set; } = new RoleDTO();

        public bool IsFirst { get; set; }
    }
}
