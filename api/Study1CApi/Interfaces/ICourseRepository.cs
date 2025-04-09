using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Study1CApi.Models;
using Study1CApi.DTOs.CourseDTOs;

namespace Study1CApi.Interfaces
{
    public interface ICourseRepository
    {
        public Task<IEnumerable<CourseDTO>> GetAllCourses();
    }
}
