using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.CourseDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.DTOs.UserDTOs;

namespace Study1CApi.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly Study1cDbContext _context;
        public CourseRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCourses()
        {
            return await _context.Courses.AsNoTracking().Select(x => new CourseDTO()
            {
                CourseId = x.CourseId,
                CourseName = x.CourseName,
                CourseDataCreate = x.CourseDataCreate,
                Description = x.Description,
                Author = new AuthorOfCourseDTO()
                {
                    UserSurname = x.AuthorNavigation.UserSurname,
                    UserName = x.AuthorNavigation.UserName,
                    UserPatronymic = x.AuthorNavigation.UserPatronymic
                }

            }).ToListAsync();
        }
    }
}
