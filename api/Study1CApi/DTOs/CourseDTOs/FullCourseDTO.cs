using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Study1CApi.DTOs.BlockDTOs;

namespace Study1CApi.DTOs.CourseDTOs
{
    public class FullCourseDTO
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public DateTime CourseDataCreate { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public ShortBlockDTO Blocks { get; set; }
    }
}
