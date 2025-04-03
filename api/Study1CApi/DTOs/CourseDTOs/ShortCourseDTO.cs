using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study1CApi.DTOs.CourseDTOs
{
    public class CourseDTO
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public DateTime CourseDataCreate { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }
    }
}
