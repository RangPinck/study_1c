using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.CourseDTOs;
using Study1CApi.DTOs.UserDTOs;
using Study1CApi.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;

        public CourseController(ICourseRepository course, IUserRepository userRepository)
        {
            _courseRepository = course;
            _userRepository = userRepository;
        }

        [SwaggerOperation(Summary = "Получение всех курсов")]
        [HttpGet("GetAllCourses")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CourseDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var courses = await _courseRepository.GetAllCourses();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение курса по id")]
        [HttpGet("GetCourseById")]
        [ProducesResponseType(200, Type = typeof(FullCourseDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetCourseById(Guid courseId)
        {
            try
            {
                var course = await _courseRepository.GetCourseById(courseId);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(course);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение возможных авторов курсов")]
        [HttpGet("GetAuthorsForCourses")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CourseAuthorDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> GetAuthorsForCourses()
        {
            try
            {
                var authors = await _userRepository.GetAuthorsForCourses();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Добавление курса")]
        [HttpPost("AddCourse")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> AddCourse(AddCourseDTO newCourse)
        {
            try
            {
                if (await _courseRepository.CourseComparisonByAuthorAndTitle(newCourse.Author, newCourse.Title))
                {
                    return BadRequest("This course already exist.");
                }

                if (string.IsNullOrEmpty(newCourse.Title))
                {
                    return BadRequest("A course cannot exist without title.");
                }

                if (string.IsNullOrEmpty(newCourse.Author.ToString()))
                {
                    return BadRequest("A course cannot exist without author.");
                }
                
                if (!await _userRepository.UserIsExist(newCourse.Author))
                {
                    return BadRequest("This user doesn't exists in database");
                }

                if (!await _courseRepository.AddCourse(newCourse))
                {
                    return BadRequest("This course doesn't add to database. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }
    }
}
