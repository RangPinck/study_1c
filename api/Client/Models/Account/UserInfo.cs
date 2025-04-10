using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Account
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserSurname { get; set; }
        public string UserName { get; set; }
        public string? UserPatronymic { get; set; }
        public bool IsFirst { get; set; }
        public List<string> UserRole { get; set; }
        public string Token { get; set; }
    }
}
