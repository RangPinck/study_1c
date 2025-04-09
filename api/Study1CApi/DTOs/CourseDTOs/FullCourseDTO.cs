using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Study1CApi.DTOs.BlockDTOs;
using Study1CApi.DTOs.UserDTOs;

namespace Study1CApi.DTOs.CourseDTOs
{
    public class FullCourseDTO
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public DateTime CourseDataCreate { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public AuthorOfCourseDTO Author { get; set; }

        public ICollection<ShortBlockDTO> Blocks { get; set; }
    }
}
