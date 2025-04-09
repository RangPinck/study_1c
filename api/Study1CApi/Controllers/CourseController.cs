using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.CourseDTOs;
using Study1CApi.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _course;

        public CourseController(ICourseRepository course)
        {
            _course = course;
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
                var courses = await _course.GetAllCourses();

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
    }
}
