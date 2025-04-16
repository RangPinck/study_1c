using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Study1CApi.DTOs.UserDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AuthUser> _userManager;

        public UserController(IUserRepository userRepository, UserManager<AuthUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [SwaggerOperation(Summary = "Получение всех пользователей")]
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var loginUser = HttpContext.User;
                var currentUser = _userManager.FindByEmailAsync(loginUser.Identity.Name).Result;
                var userRoles = _userManager.GetRolesAsync(currentUser).Result.ToList();

                var users = await _userRepository.GetAllUsersAsync(userRoles.Contains("Администратор"));

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                Log.Error($"get all users => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }
    }
}
