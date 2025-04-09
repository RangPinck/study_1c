using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.CourseDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.DTOs.UserDTOs;
using Study1CApi.DTOs.BlockDTOs;
using Microsoft.AspNetCore.Identity;

namespace Study1CApi.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly Study1cDbContext _context;
        private readonly UserManager<AuthUser> _userManager;

        public CourseRepository(Study1cDbContext context, UserManager<AuthUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public async Task<FullCourseDTO> GetCourseById(Guid courseId)
        {
            return await _context.Courses.AsNoTracking().Select(course => new FullCourseDTO()
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseDataCreate = course.CourseDataCreate,
                Description = course.Description,
                Link = course.Link,
                Author = new AuthorOfCourseDTO()
                {
                    UserSurname = course.AuthorNavigation.UserSurname,
                    UserName = course.AuthorNavigation.UserName,
                    UserPatronymic = course.AuthorNavigation.UserPatronymic
                },
                Blocks = course.CoursesBlocks.Select(block => new ShortBlockDTO()
                {
                    BlockId = block.BlockId,
                    BlockName = block.BlockName,
                    BlockDateCreated = block.BlockDateCreated,
                    Description = block.Description,
                    BlockNumberOfCourse = block.BlockNumberOfCourse
                }).ToList()
            }).FirstOrDefaultAsync(x => x.CourseId == courseId);
        }

        public async Task<IEnumerable<CourseAuthorDTO>> GetAuthorsForCourses()
        {
            List<Guid> users = _userManager.GetUsersInRoleAsync("Администратор").Result.Select(x => x.Id).ToList();
            users = [.. users, .. _userManager.GetUsersInRoleAsync("Куратор").Result.Select(x => x.Id).Distinct().ToList()];

            return await _context.Users.AsNoTracking().Where(x => users.Contains(x.UserId)).Select(x => new CourseAuthorDTO()
            {
                UserId = x.UserId,
                UserName = x.UserName,
                UserSurname = x.UserSurname,
                UserPatronymic = x.UserPatronymic
            }).ToListAsync();
        }
    }
}
