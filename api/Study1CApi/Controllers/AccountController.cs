using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Study1CApi.DTOs.AccountDTOs;
using Study1CApi.DTOs.AuthDTO;
using Study1CApi.DTOs.UserDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                if (appUser == null) return Unauthorized("There is no such user in the database!");

                var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);

                if (!result.Succeeded) return Unauthorized("Incorrect password!");

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

                Log.Information($"loggin => {profile.Id}");

                return Ok(profile);
            }
            catch (Exception e)
            {
                Log.Error($"loggin => {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        [SwaggerOperation(Summary = "Создание пользователя")]
        [HttpPost("Register")]
        [Authorize(Roles = "Администратор, Куратор")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO registerDto)
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
                                IsFirst = !(role.Name == "Куратор" || role.Name == "Администратор"),
                                UserRole = new List<string> { role.Name }
                            };

                            Log.Information($"registration => {newUserDTO.UserId}");

                            return Ok(newUserDTO);
                        }
                        else
                        {
                            Log.Error($"registration => User creation error!");
                            return BadRequest("User creation error!");
                        }
                    }
                    else
                    {
                        Log.Error($"registration => {roleResult.Errors.FirstOrDefault().Description}");
                        return BadRequest(roleResult.Errors.FirstOrDefault().Description);
                    }
                    ;
                }
                else
                {
                    var error = createdUser.Errors.FirstOrDefault();
                    if (error != null)
                        if (error.Code == "DuplicateUserName")
                        {
                            Log.Error($"registration => There is already such a user!");
                            return BadRequest("There is already such a user!");
                        }
                    Log.Error($"registration => {error.Description}");
                    return BadRequest(error.Description);
                }
            }
            catch (Exception e)
            {
                Log.Error($"registration => {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        [SwaggerOperation(Summary = "Обновление профиля пользователя")]
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

                if (appUser == null) return Unauthorized("There is no such user in the database!");

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (authUser.Id == updateUser.UserId)
                {
                    if (await _account.UpdateUserProfileAsync(updateUser))
                    {
                        var result = await _userManager.SetEmailAsync(authUser, updateUser.Email);
                        authUser.NormalizedEmail = updateUser.Email.ToUpper();
                        authUser.UserName = updateUser.UserName + " " + updateUser.UserName + " " + updateUser.UserPatronymic;
                        authUser.NormalizedUserName = authUser.UserName.ToUpper();
                        authUser.ConcurrencyStamp = DateTime.UtcNow.ToString();
                        result = await _userManager.UpdateAsync(authUser);

                        if (!result.Succeeded)
                        {
                            Log.Error($"update profile => Incorrect data!");
                            return BadRequest("Incorrect data!");
                        }
                    }
                    else
                    {
                        Log.Error($"update profile => Incorrect data!");
                        return BadRequest("Incorrect data!");
                    }
                }
                else
                {
                    if (authUserRoles.Contains("Администратор") || authUserRoles.Contains("Куратор"))
                    {
                        var userWillBeUpdate = await _userManager.FindByIdAsync(updateUser.UserId.ToString());
                        var userWillBeUpdateRoles = _userManager.GetRolesAsync(userWillBeUpdate).Result.ToList();

                        if ((authUserRoles.Contains("Администратор") && userWillBeUpdateRoles.Contains("Администратор")) || (authUserRoles.Contains("Куратор") && userWillBeUpdateRoles.Contains("Куратор")))
                        {
                            return BadRequest("You don't have enough rights for this operation!");
                        }

                        if (await _account.UpdateUserProfileAsync(updateUser))
                        {
                            var result = await _userManager.SetEmailAsync(userWillBeUpdate, updateUser.Email);

                            if (!result.Succeeded) return BadRequest("Incorrect email!");

                            userWillBeUpdate.NormalizedEmail = updateUser.Email.ToUpper();
                            userWillBeUpdate.UserName = updateUser.Email.ToLower();
                            userWillBeUpdate.NormalizedUserName = updateUser.Email.ToUpper();
                            userWillBeUpdate.ConcurrencyStamp = DateTime.UtcNow.ToString();
                            result = await _userManager.UpdateAsync(userWillBeUpdate);

                            if (!result.Succeeded) return BadRequest("Incorrect data!");
                        }
                        else return BadRequest("Incorrect data!");
                    }
                    else return BadRequest("You don't have enough rights for this operation!");
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

        [SwaggerOperation(Summary = "Обновление пароля пользователя")]
        [HttpPut("UpdatePasswordForProfile")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePasswordForProfile(UpdatePasswordDTO updateDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();
                var appUser = _userManager.FindByEmailAsync(updateDTO.Email).Result;
                var appUserRoles = _userManager.GetRolesAsync(appUser).Result.ToList();

                if (appUser == null) return BadRequest("The user with this email does not exist!");

                if (authUser.Email == updateDTO.Email || (authUserRoles.Contains("Администратор") && !appUserRoles.Contains("Администратор")))
                {
                    if (updateDTO.Password == updateDTO.ConfirmPassword)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                        var result = await _userManager.ResetPasswordAsync(appUser, token, updateDTO.Password);

                        if (!result.Succeeded) return BadRequest("The password could not be updated!");
                    }
                    else return BadRequest("Passwords don't match!");
                }
                else return BadRequest("You don't have enough rights for this operation!");

                return Ok("The password has been updated!");
            }
            catch (Exception ex)
            {
                Log.Error($"update password => {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Обновление роли пользователя")]
        [HttpPut("UpdateRoleProfile")]
        [Authorize(Roles = "Администратор")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRoleProfile(UpdateUserRoleDTO updateRole)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var appUser = await _userManager.FindByIdAsync(updateRole.UserId.ToString());

                if (appUser == null) return BadRequest("The user with this id does not exist in the database!");

                var userRole = await _roleManager.FindByIdAsync(updateRole.RoleId.ToString());

                if (userRole == null) return BadRequest("The role with this id does not exist in the database!");

                var appUserRoles = await _userManager.GetRolesAsync(appUser);

                if (appUserRoles.Contains("Администратор")) return BadRequest("You don't have enough rights for this operation!");

                var result = await _userManager.RemoveFromRoleAsync(appUser, appUserRoles.FirstOrDefault());

                if (!result.Succeeded) return BadRequest("Couldn't delete the old user role!");

                result = await _userManager.AddToRoleAsync(appUser, userRole.Name);

                if (!result.Succeeded) return BadRequest("Couldn't install a new role for the user!");

                return Ok("The role has been updated!");
            }
            catch (Exception ex)
            {
                Log.Error($"update role user => {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Регистрация первого входа пользователя")]
        [HttpPut("RegistrationUserFirstLogin")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RegistrationUserFirstLogin()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;

                var appUser = await _userManager.FindByIdAsync(authUser.Id.ToString());

                if (appUser == null) return BadRequest("There is no such user in the database!");

                if (!await _account.RegistrationUserFirstLoginAsync(appUser.Id)) return BadRequest("Incorrect data!");

                return Ok("The user is logged in!");
            }
            catch (Exception ex)
            {
                Log.Error($"user first login => {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Удаление пользователя")]
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

                var appUser = await _userManager.FindByIdAsync(deleteUser.UserId.ToString());

                if (appUser == null) return BadRequest("There is no such user in the database!");

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                var listAppUserRoles = _userManager.GetRolesAsync(appUser).Result.ToList();

                if (listAppUserRoles.Contains("Администратор") && !authUserRoles.Contains("Администратор"))
                {
                    return BadRequest("You don't have enough rights for this operation!");
                }

                if (_userManager.GetUsersInRoleAsync("Администратор").Result.ToList().Count == 1 &&
                    _userManager.GetRolesAsync(appUser).Result.Any(x => x == "Администратор"))
                {
                    return BadRequest("You cannot delete the last administrator!");
                }

                if (appUser == authUser || authUserRoles.Contains("Администратор"))
                {
                    var result = await _userManager.DeleteAsync(appUser);

                    if (!result.Succeeded) return BadRequest("The operation failed!");
                }

                return Ok("The user has been deleted!");
            }
            catch (Exception ex)
            {
                Log.Error($"delete profile => {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
