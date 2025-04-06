using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.AuthDTO;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly Study1cDbContext _context;


        public AccountController(UserManager<AuthUser> userManager, ITokenService tokenService,
            SignInManager<AuthUser> signInManager, RoleManager<Role> roleManager, Study1cDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [SwaggerOperation(Summary = "Создание пользователя")]
        [HttpPost("Register")]
        //[Authorize(Roles = "Администратор")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                    return BadRequest(new { message = firstError });
                }

                var appUser = new AuthUser
                {
                    Email = registerDto.Email.ToLower(),
                    UserName = registerDto.Email.ToLower(),
                    EmailConfirmed = true
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "Ученик");

                    if (roleResult.Succeeded)
                    {
                        var user = await _userManager.FindByEmailAsync(registerDto.Email.ToLower());

                        if (user != null)
                        {
                            User newUser = new()
                            {
                                UserId = appUser.Id,
                                IsFirst = registerDto.IsFirst,
                                UserName = registerDto.UserName,
                                UserSurname = registerDto.UserSurname,
                                UserPatronymic = registerDto.UserPatronymic,
                                AuthUserNavigation = appUser
                            };

                            _context.Users.Add(newUser);
                            _context.SaveChanges();

                            return Ok(newUser);
                        }
                        else return StatusCode(400, "Ошибка создания пользователя");
                    }
                    else return StatusCode(400, roleResult.Errors.FirstOrDefault().Description);
                }
                else
                {
                    var error = createdUser.Errors.FirstOrDefault();
                    if (error != null)
                        if (error.Code == "DuplicateUserName")
                            return BadRequest("Такой пользователь уже есть");
                    return StatusCode(400, error.Description);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [SwaggerOperation(Summary = "Авторизация в системе")]
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = await _userManager.FindByEmailAsync(loginDto.Email.ToLower());

            if (appUser == null) return Unauthorized("Такого пользователя нет в базе");

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Неверный пароль");

            var userRole = await _userManager.GetRolesAsync(appUser);

            var user = _context.Users.FirstOrDefault(u => u.UserId == appUser.Id);

            SignInDTO profile = new SignInDTO()
            {
                Email = loginDto.Email,
                UserSurname = user.UserSurname,
                UserName = user.UserName,
                UserPatronymic = user.UserPatronymic,
                IsFirst = user.IsFirst,
                Token = _tokenService.CreateToken(appUser, userRole.First())
            };

            return Ok(profile);
        }
    }
}
