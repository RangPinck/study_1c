using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Study1CApi.Models;
using Study1CApi.DTOs.CourseDTOs;
using Study1CApi.DTOs.UserDTOs;

namespace Study1CApi.Interfaces
{
    public interface ICourseRepository
    {
        public Task<IEnumerable<ShortCourseDTO>> GetAllCourses();

        public Task<FullCourseDTO> GetCourseById(Guid courseId);

        public Task<bool> AddCourse(AddCourseDTO newCourse);

        public Task<bool> UpdateCourse(UpdateCourseDTO updatedCourse);

        public Task<bool> SaveChangesAsync();

        public Task<bool> CourseComparisonByAuthorAndTitle(Guid authorId, string courseTitle);

        public Task<StandardCourseDTO> GetCourseDataById(Guid courseId);
    }
}
