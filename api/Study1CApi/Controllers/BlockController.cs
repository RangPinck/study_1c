using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.BlockDTOs;
using Study1CApi.DTOs.CourseDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IBlockRepository _blockRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AuthUser> _userManager;

        public BlockController(IBlockRepository blockRepository, IUserRepository userRepository, ICourseRepository courseRepository, UserManager<AuthUser> userManager)
        {
            _blockRepository = blockRepository;
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _userManager = userManager;
        }

        [SwaggerOperation(Summary = "Получение блоков (разделов) курса")]
        [HttpGet("GetBlockOfCourse")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ShortBlockDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetBlockOfCourse(Guid courseId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();
                var subCheck = new SubscribeUserCourseDTO()
                {
                    courseId = courseId,
                    userId = authUser.Id,
                };

                IEnumerable<ShortBlockDTO> blocks = new List<ShortBlockDTO>();

                if (!authUserRoles.Contains("Администратор") && !authUserRoles.Contains("Куратор") && !await _courseRepository.CheckUserSubscribeOnCourse(subCheck))
                {
                    return BadRequest("User doesnt subscribe to course!");
                }
                else
                {
                    blocks = await _blockRepository.GetBlocksOfCourseAsync(courseId);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(blocks);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        //[SwaggerOperation(Summary = "Добавление блока")]
        //[HttpPost("AddBlock")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(500)]
        //[Authorize(Roles = "Администратор, Куратор")]
        //public async Task<IActionResult> AddBlock(AddCourseDTO newCourse)
        //{
        //    try
        //    {
        //        if (await _courseRepository.CourseComparisonByAuthorAndTitle(newCourse.Author, newCourse.Title))
        //        {
        //            return BadRequest("This course already exist.");
        //        }

        //        if (string.IsNullOrEmpty(newCourse.Title))
        //        {
        //            return BadRequest("A course cannot exist without title.");
        //        }

        //        if (string.IsNullOrEmpty(newCourse.Author.ToString()))
        //        {
        //            return BadRequest("A course cannot exist without author.");
        //        }

        //        if (!await _userRepository.UserIsExist(newCourse.Author))
        //        {
        //            return BadRequest("This user doesn't exists in database");
        //        }

        //        if (!await _courseRepository.AddCourse(newCourse))
        //        {
        //            return BadRequest("This course doesn't add to database. No correct data.");
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();
        //        }

        //        return Ok("Operation success");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(503, ex.Message);
        //    }
        //}
    }
}
