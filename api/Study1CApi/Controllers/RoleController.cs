using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.RoleDTOs;
using Study1CApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        [SwaggerOperation(Summary = "Получение всех ролей")]
        [HttpGet("GetAllRoles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoleDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        [AllowAnonymous]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.Select(role => new RoleDTO()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                }).ToListAsync();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Добавление роли")]
        [HttpPost("AddRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        [AllowAnonymous]
        public async Task<IActionResult> AddRole(string newRoleName)
        {
            try
            {
                if (await _roleManager.RoleExistsAsync(newRoleName))
                {
                    return BadRequest("This role already exist.");
                }

                if (string.IsNullOrEmpty(newRoleName))
                {
                    return BadRequest("A role cannot exist without title.");
                }

                var roleWasCreated = await _roleManager.CreateAsync(new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = newRoleName,
                    NormalizedName = newRoleName.ToUpper(),
                    ConcurrencyStamp = DateTime.Now.ToString(),
                    IsNoManipulate = false
                });

                if (!roleWasCreated.Succeeded)
                {
                    return BadRequest($"This role doesn't add to database. No correct data.\n{roleWasCreated.Errors.FirstOrDefault().Description}");
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

        [SwaggerOperation(Summary = "Обновление роли")]
        [HttpPut("UpdateRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        [AllowAnonymous]
        public async Task<IActionResult> UpdateRole(Guid roleId, string roleName)
        {
            try
            {
                if (await _roleManager.FindByIdAsync(roleId.ToString()) is null)
                {
                    return BadRequest("This role cannot exist.");
                }

                if (await _roleManager.FindByNameAsync(roleName) != null)
                {
                    return BadRequest("Role with currant name already exist.");
                }

                if (string.IsNullOrEmpty(roleName))
                {
                    return BadRequest("A role cannot exist without title.");
                }

                var updateRole = await _roleManager.FindByIdAsync(roleId.ToString());

                if (updateRole.IsNoManipulate)
                {
                    return BadRequest("it is forbidden to change this role!");
                }

                updateRole.Name = roleName;
                updateRole.NormalizedName = roleName.ToUpper();
                updateRole.ConcurrencyStamp = DateTime.Now.ToString();

                var updateRoleResult = await _roleManager.UpdateAsync(updateRole);

                if (!updateRoleResult.Succeeded)
                {
                    return BadRequest($"This role doesn't update. No correct data.\n{updateRoleResult.Errors.FirstOrDefault().Description}");
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

        [SwaggerOperation(Summary = "Удаление роли")]
        [HttpDelete("DeleteRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        [AllowAnonymous]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            try
            {
                var deleteRole = await _roleManager.FindByIdAsync(roleId.ToString());

                if (deleteRole is null)
                {
                    return BadRequest("This role cannot exist.");
                }

                if (deleteRole.IsNoManipulate)
                {
                    ModelState.AddModelError("", "This role doesn't delete. No correct data.");
                    return StatusCode(400, ModelState);
                }

                var isDeleteRole = await _roleManager.DeleteAsync(deleteRole);

                if (!isDeleteRole.Succeeded)
                {
                    return BadRequest($"This role doesn't delete. No correct data.\n{isDeleteRole.Errors.FirstOrDefault().Description}");
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
