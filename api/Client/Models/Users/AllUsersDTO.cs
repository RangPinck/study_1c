using System;
using System.Collections.Generic;

namespace Client.Models.Users
{
    public class UserDTO
    {
        public Guid UserId { get; set; }

        public string UserLogin { get; set; } = string.Empty;

        public string UserSurname { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? UserPatronymic { get; set; }

        public bool IsFirst { get; set; }

        public List<string> UserRole { get; set; } = new List<string>();

        public DateTime UserDataCreate { get; set; }
    }
}
