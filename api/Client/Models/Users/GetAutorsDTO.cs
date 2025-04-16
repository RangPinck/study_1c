using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Users
{
    public class CourseAuthorDTO
    {
        public Guid UserId { get; set; }

        public string UserSurname { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? UserPatronymic { get; set; }

    }
}
