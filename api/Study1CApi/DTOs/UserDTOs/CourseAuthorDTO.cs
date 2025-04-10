using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study1CApi.DTOs.UserDTOs
{
    public class CourseAuthorDTO
    {
        public Guid UserId { get; set; }

        public string UserSurname { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? UserPatronymic { get; set; }
    }
}
