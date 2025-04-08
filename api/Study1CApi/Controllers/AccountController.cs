using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.AccountDTOs;
using Study1CApi.DTOs.AuthDTO;
using Study1CApi.DTOs.UserDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

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
        private readonly IAccountRepository _account;

        public AccountController(UserManager<AuthUser> userManager, ITokenService tokenService,
            SignInManager<AuthUser> signInManager, RoleManager<Role> roleManager, Study1cDbContext context, IAccountRepository account)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _account = account;
        }

        [SwaggerOperation(Summary = "Создание пользователя")]
        [HttpPost("Register")]
        [Authorize(Roles = "Администратор, Куратор")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegistartionDTO registerDto)
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
                    Email = registerDto.Email,
                    UserName = registerDto.Email.ToLower(),
                    EmailConfirmed = true,
                    UserDataCreate = DateTime.UtcNow,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var role = await _roleManager.FindByIdAsync(registerDto.RoleId.ToString());

                    if (role == null)
                    {
                        return BadRequest("Role doesn't exists");
                    }

                    var roleResult = await _userManager.AddToRoleAsync(appUser, role.Name);

                    if (roleResult.Succeeded)
                    {
                        var user = await _userManager.FindByEmailAsync(registerDto.Email.ToLower());

                        if (user != null)
                        {
                            User newUser = new()
                            {
                                UserId = user.Id,
                                IsFirst = registerDto.IsFirst,
                                UserName = registerDto.UserName,
                                UserSurname = registerDto.UserSurname,
                                UserPatronymic = registerDto.UserPatronymic,
                                AuthUserNavigation = user
                            };

                            _context.Users.Add(newUser);
                            _context.SaveChanges();

                            var newUserDTO = new UserDTO()
                            {
                                UserId = user.Id,
                                UserLogin = user.Email,
                                UserName = registerDto.UserName,
                                UserSurname = registerDto.UserSurname,
                                UserPatronymic = registerDto.UserPatronymic,
                                UserDataCreate = user.UserDataCreate,
                                IsFirst = ! (role.Name == "Куратор" || role.Name == "Администратор"),
                                UserRole = new List<string> { role.Name }
                            };

                            return Ok(newUserDTO);
                        }
                        else return BadRequest("Ошибка создания пользователя");
                    }
                    else return BadRequest(roleResult.Errors.FirstOrDefault().Description);
                }
                else
                {
                    var error = createdUser.Errors.FirstOrDefault();
                    if (error != null)
                        if (error.Code == "DuplicateUserName")
                            return BadRequest("Такой пользователь уже есть");
                    return BadRequest(error.Description);
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
        [ProducesResponseType(200, Type = typeof(SignInDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = await _userManager.FindByEmailAsync(loginDto.Email);

                if (appUser == null) return Unauthorized("Такого пользователя нет в базе");

                var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);

                if (!result.Succeeded) return Unauthorized("Неверный пароль");

                var userRole = await _userManager.GetRolesAsync(appUser);

                var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserId == appUser.Id);

                SignInDTO profile = new SignInDTO()
                {
                    Id = user.UserId,
                    Email = loginDto.Email,
                    UserSurname = user.UserSurname,
                    UserName = user.UserName,
                    UserPatronymic = user.UserPatronymic,
                    UserRole = userRole.ToList(),
                    IsFirst = user.IsFirst,
                    Token = _tokenService.CreateToken(appUser, userRole.First())
                };

                return Ok(profile);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [SwaggerOperation(Summary = "Удаление прользователя")]
        [HttpDelete("DeleteAccount")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAccount(DeleteAccountDTO deleteUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var loginUser = await _userManager.FindByIdAsync(deleteUser.UserId.ToString());

                if (loginUser == null) return Unauthorized("Пользователь не вошёл в систему!");

                var appUser = await _userManager.FindByIdAsync(deleteUser.UserIdWillBeDelete.ToString());

                if (appUser == null) return BadRequest("Такого пользователя нет в базе");

                var listLoginUserRoles = _userManager.GetRolesAsync(loginUser).Result.ToList();
                var listAppUserRoles = _userManager.GetRolesAsync(appUser).Result.ToList();

                if (listAppUserRoles.Any(x => x == "Администратор") && !listLoginUserRoles.Any(x => x == "Администратор"))
                {
                    return BadRequest("У вас не достаточно прав, чтобы удалить данного пользователя!");
                }

                if (_userManager.GetUsersInRoleAsync("Администратор").Result.ToList().Count == 1 &&
                    _userManager.GetRolesAsync(appUser).Result.Any(x => x == "Администратор"))
                {
                    return BadRequest("Вы не можете удалить послежнего администратора!");
                }

                if (appUser == loginUser)
                {
                    await _userManager.DeleteAsync(appUser);
                }

                if (listLoginUserRoles.Any(x => x == "Администратор"))
                {
                    await _userManager.DeleteAsync(appUser);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Обновление профиля прользователя")]
        [HttpPut("UpdateProfile")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDTO updateUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = await _userManager.FindByIdAsync(updateUser.UserId.ToString());

                if (appUser == null) return Unauthorized("Такого пользователя нет в базе!");

                var httpUser = HttpContext.User;
                var authtUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authtUser).Result.ToList();

                if (authtUser.Id == updateUser.UserId)
                {
                    await _account.UpdateUserProfile(updateUser);
                    authtUser.Email = updateUser.Email;
                    authtUser.NormalizedEmail = updateUser.Email.ToUpper();
                    authtUser.UserName = updateUser.UserName + " " + updateUser.UserName + " " + updateUser.UserPatronymic;
                    authtUser.NormalizedUserName = authtUser.UserName.ToUpper();
                    authtUser.ConcurrencyStamp = DateTime.UtcNow.ToString();
                    await _userManager.UpdateAsync(authtUser);
                }
                else
                {
                    if (authUserRoles.Contains("Администратор") || authUserRoles.Contains("Куратор"))
                    {
                        var userWillBeUpdate = await _userManager.FindByIdAsync(updateUser.UserId.ToString());
                        var userWillBeUpdateRoles = _userManager.GetRolesAsync(userWillBeUpdate).Result.ToList();

                        if ((authUserRoles.Contains("Администратор") && userWillBeUpdateRoles.Contains("Администратор")) || (authUserRoles.Contains("Куратор") && userWillBeUpdateRoles.Contains("Куратор")))
                        {
                            return BadRequest("У вас не достаточно прав для изменения данного пользователя!");
                        }

                        await _account.UpdateUserProfile(updateUser);
                        userWillBeUpdate.Email = updateUser.Email;
                        userWillBeUpdate.NormalizedEmail = updateUser.Email.ToUpper();
                        userWillBeUpdate.UserName = updateUser.UserName + " " + updateUser.UserName + " " + updateUser.UserPatronymic;
                        userWillBeUpdate.NormalizedUserName = userWillBeUpdate.UserName.ToUpper();
                        userWillBeUpdate.ConcurrencyStamp = DateTime.UtcNow.ToString();
                        await _userManager.UpdateAsync(userWillBeUpdate);
                    }
                    else
                    {
                        return BadRequest("У вас не достаточно прав для изменения данного пользователя!");
                    }
                }

                var userWasUpdate = await _userManager.FindByIdAsync(updateUser.UserId.ToString());
                var role = _userManager.GetRolesAsync(userWasUpdate).Result.ToList();

                var response = new UserDTO()
                {
                    UserId = updateUser.UserId,
                    UserLogin = updateUser.Email,
                    UserName = updateUser.UserName,
                    UserSurname = updateUser.UserSurname,
                    UserPatronymic = updateUser.UserPatronymic,
                    UserDataCreate = userWasUpdate.UserDataCreate,
                    UserRole = role
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Обновление пароля прользователя")]
        [HttpPut("UpdatePasswordForProfile")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePasswordForProfile(DeleteAccountDTO deleteUser)
        {
            try
            {
                //if (!ModelState.IsValid)
                //    return BadRequest(ModelState);

                //var loginUser = await _userManager.FindByIdAsync(deleteUser.UserId.ToString());

                //if (loginUser == null) return Unauthorized("Пользователь не вошёл в систему!");

                //var appUser = await _userManager.FindByIdAsync(deleteUser.UserIdWillBeDelete.ToString());

                //if (appUser == null) return BadRequest("Такого пользователя нет в базе");

                //var listLoginUserRoles = _userManager.GetRolesAsync(loginUser).Result.ToList();
                //var listAppUserRoles = _userManager.GetRolesAsync(appUser).Result.ToList();

                //if (listAppUserRoles.Any(x => x == "Администратор") && !listLoginUserRoles.Any(x => x == "Администратор"))
                //{
                //    return BadRequest("У вас не достаточно прав, чтобы удалить данного пользователя!");
                //}

                //if (_userManager.GetUsersInRoleAsync("Администратор").Result.ToList().Count == 1 &&
                //    _userManager.GetRolesAsync(appUser).Result.Any(x => x == "Администратор"))
                //{
                //    return BadRequest("Вы не можете удалить послежнего администратора!");
                //}

                //if (appUser == loginUser)
                //{
                //    await _userManager.DeleteAsync(appUser);
                //}

                //if (listLoginUserRoles.Any(x => x == "Администратор"))
                //{
                //    await _userManager.DeleteAsync(appUser);
                //}

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Обновление роли прользователя")]
        [HttpPut("UpdateRoleProfile")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRoleProfile(DeleteAccountDTO deleteUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var loginUser = await _userManager.FindByIdAsync(deleteUser.UserId.ToString());

                if (loginUser == null) return Unauthorized("Пользователь не вошёл в систему!");

                var appUser = await _userManager.FindByIdAsync(deleteUser.UserIdWillBeDelete.ToString());

                if (appUser == null) return BadRequest("Такого пользователя нет в базе");

                var listLoginUserRoles = _userManager.GetRolesAsync(loginUser).Result.ToList();
                var listAppUserRoles = _userManager.GetRolesAsync(appUser).Result.ToList();

                if (listAppUserRoles.Any(x => x == "Администратор") && !listLoginUserRoles.Any(x => x == "Администратор"))
                {
                    return BadRequest("У вас не достаточно прав, чтобы удалить данного пользователя!");
                }

                if (_userManager.GetUsersInRoleAsync("Администратор").Result.ToList().Count == 1 &&
                    _userManager.GetRolesAsync(appUser).Result.Any(x => x == "Администратор"))
                {
                    return BadRequest("Вы не можете удалить послежнего администратора!");
                }

                if (appUser == loginUser)
                {
                    await _userManager.DeleteAsync(appUser);
                }

                if (listLoginUserRoles.Any(x => x == "Администратор"))
                {
                    await _userManager.DeleteAsync(appUser);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Регистрация первого входа прользователя")]
        [HttpPut("RegistrateUserFirstLogin")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RegistrateUserFirstLogin()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var httpUser = HttpContext.User;
                var authtUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;

                var appUser = await _userManager.FindByIdAsync(authtUser.Id.ToString());

                if (appUser == null) return BadRequest("Такого пользователя нет в базе");

                if (!await _account.RegistrateUserFirstLogin(appUser.Id))
                {
                    return BadRequest("Не корректные данные");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
