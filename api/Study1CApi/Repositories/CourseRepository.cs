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

        public async Task<IEnumerable<ShortCourseDTO>> GetAllCourses()
        {
            return await _context.Courses.AsNoTracking().Select(x => new ShortCourseDTO()
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

        public async Task<bool> AddCourse(AddCourseDTO newCourse)
        {
            var course = new Course()
            {
                CourseName = newCourse.Title,
                CourseDataCreate = DateTime.UtcNow.AddHours(3),
                Description = newCourse.Description,
                Link = newCourse.Link,
                Author = newCourse.Author
            };

            await _context.Courses.AddAsync(course);

            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<bool> CourseComparisonByAuthorAndTitle(Guid authorId, string courseTitle)
        {
            return await _context.Courses.AnyAsync(x => x.CourseName == courseTitle && x.Author == authorId);
        }

        public async Task<bool> UpdateCourse(UpdateCourseDTO updatedCourse)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == updatedCourse.CourseId);

            if (course == null)
            {
                return false;
            }

            course.CourseName = updatedCourse.Title;
            course.Description = updatedCourse.Description;
            course.Link = updatedCourse.Link;

            _context.Courses.Update(course);

            return await SaveChangesAsync();
        }

        public async Task<StandardCourseDTO> GetCourseDataById(Guid courseId)
        {
            var course = await _context.Courses.Select(x => new StandardCourseDTO()
            {
                CourseId = x.CourseId,
                CourseName = x.CourseName,
                Description = x.Description,
                Link = x.Link,
                Author = x.Author,
            }).FirstOrDefaultAsync(x => x.CourseId == courseId);

            return course;
        }

        public async Task<bool> DeleteCourse(Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == courseId);
            _context.Courses.Remove(course);
            return await SaveChangesAsync();
        }

        public async Task<bool> CheckSubsOnCourse(Guid courseId)
        {
            return await _context.UserCourses.AnyAsync(x => x.CourseId == courseId);
        }
    }
}
