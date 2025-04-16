using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Study1CApi.DTOs.CourseDTOs;
using Study1CApi.DTOs.UserDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AuthUser> _userManager;

        public CourseController(ICourseRepository course, IUserRepository userRepository, UserManager<AuthUser> userManager)
        {
            _courseRepository = course;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [SwaggerOperation(Summary = "Получение всех курсов")]
        [HttpGet("GetAllCourses")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ShortCourseDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                IEnumerable<ShortCourseDTO> courses = new List<ShortCourseDTO>();

                if (authUserRoles.Contains("Администратор"))
                {
                    courses = await _courseRepository.GetAllCoursesAsync();
                }
                else
                {
                    if (authUserRoles.Contains("Куратор"))
                    {
                        courses = await _courseRepository.GetCoursesThatUserCreateAsync(authUser.Id);
                    }
                    else
                    {
                        courses = await _courseRepository.GetCoursesThatUserSubscribeAsync(authUser.Id);
                    }
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(courses);
            }
            catch (Exception ex)
            {
                Log.Error($"get all course => {ex.Message}");
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
                var course = await _courseRepository.GetCourseByIdAsync(courseId);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(course);
            }
            catch (Exception ex)
            {
                Log.Error($"get course by id => {ex.Message}");
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
                var authors = await _userRepository.GetAuthorsForCoursesAsync();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(authors);
            }
            catch (Exception ex)
            {
                Log.Error($"get authors for course => {ex.Message}");
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
                if (await _courseRepository.CourseComparisonByAuthorAndTitleAsync(newCourse.Author, newCourse.Title))
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

                if (!await _userRepository.UserIsExistAsync(newCourse.Author))
                {
                    return BadRequest("This user doesn't exists in database");
                }

                if (!await _courseRepository.AddCourseAsync(newCourse))
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
                Log.Error($"add course => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Подписка пользователя на курс")]
        [HttpPost("SubscribeUserForACourse")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> SubscribeUserForACourse(SubscribeUserCourseDTO suc)
        {
            try
            {
                var courseData = await _courseRepository.GetCourseDataByIdAsync(suc.courseId);
                if (courseData == null)
                {
                    return BadRequest("This course doesn't exist.");
                }

                var user = await _userManager.FindByIdAsync(suc.userId.ToString());

                if (user == null)
                {
                    return BadRequest("User not found!");
                }

                if (await _courseRepository.CheckUserSubscribeOnCourseAsync(suc))
                {
                    return BadRequest("User already subscribe!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != courseData.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _courseRepository.SubscribeUserForACourseAsync(suc))
                {
                    return BadRequest("This user doesn't subscribe to course. No correct data.");
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"subscribe user to course => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Обновление курса")]
        [HttpPut("UpdateCourse")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> UpdateCourse(UpdateCourseDTO updatedCourse)
        {
            try
            {
                var course = await _courseRepository.GetCourseDataByIdAsync(updatedCourse.CourseId);

                if (course == null)
                {
                    return BadRequest("This course not found.");
                }

                if (string.IsNullOrEmpty(updatedCourse.Title))
                {
                    return BadRequest("A course cannot exist without title.");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != course.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _courseRepository.UpdateCourseAsync(updatedCourse))
                {
                    return BadRequest("This course doesn't update on database. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"update course => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Удаление курса")]
        [HttpDelete("DeleteCourse")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            try
            {
                var deleteCourse = await _courseRepository.GetCourseDataByIdAsync(courseId);

                if (deleteCourse is null)
                {
                    return BadRequest("This course cannot exist.");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != deleteCourse.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (await _courseRepository.CheckSubsOnCourseAsync(courseId))
                {
                    return BadRequest("This course doesn't delete. Users have subscribed to this course.");
                }

                if (!await _courseRepository.DeleteCourseAsync(courseId))
                {
                    return BadRequest("This course doesn't delete. No correct data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"delete course => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Отписка пользователя от курса")]
        [HttpDelete("UnsubscribeUserForACourse")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> UnsubscribeUserForACourse(SubscribeUserCourseDTO suc)
        {
            try
            {
                var courseData = await _courseRepository.GetCourseDataByIdAsync(suc.courseId);
                if (courseData == null)
                {
                    return BadRequest("This course doesn't exist.");
                }

                var user = await _userManager.FindByIdAsync(suc.userId.ToString());

                if (user == null)
                {
                    return BadRequest("User not found!");
                }

                if (!await _courseRepository.CheckUserSubscribeOnCourseAsync(suc))
                {
                    return BadRequest("User doesn't subscribe!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != courseData.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _courseRepository.UnsubscribeUserForACourseAsync(suc))
                {
                    return BadRequest("This user doesn't unsubscribe of course. No correct data.");
                }

                return Ok("Operation success");
            }
            catch (Exception ex)
            {
                Log.Error($"unsubscribe user to course => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }
    }
}
