using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.RoleDTOs;
using Study1CApi.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private ICollection<RoleDTO> nonManipulatedRoles = new List<RoleDTO>(){
            new RoleDTO(){ RoleId = 1, RoleName = "Ученик"},
            new RoleDTO(){ RoleId = 2, RoleName = "Куратор"},
            new RoleDTO(){ RoleId = 3, RoleName = "Администратор"}
        };
        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [SwaggerOperation(Summary = "Получение всех ролей")]
        [HttpGet("GetAllRoles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoleDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllRoles();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(roles);
        }

        [SwaggerOperation(Summary = "Получение роли по id")]
        [HttpGet("GetRoleById")]
        [ProducesResponseType(200, Type = typeof(RoleDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            if (!await _roleRepository.RoleIsExistOfId(roleId))
            {
                return BadRequest("This role cannot exist.");
            }

            var roles = await _roleRepository.GetRoleById(roleId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(roles);
        }

        [SwaggerOperation(Summary = "Добавление роли")]
        [HttpPost("AddRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddBook(string newRoleName)
        {
            if (await _roleRepository.RoleIsExistOfName(newRoleName))
            {
                return BadRequest("This role already exist.");
            }

            if (string.IsNullOrEmpty(newRoleName))
            {
                return BadRequest("A role cannot exist without title.");
            }

            if (!await _roleRepository.AddRole(new NewRoleDTO() { RoleName = newRoleName }))
            {
                ModelState.AddModelError("", "This role doesn't add to database. No correct data.");
                return StatusCode(400, ModelState);
            }

            return Ok("Operation success");
        }

        [SwaggerOperation(Summary = "Обновление роли")]
        [HttpPut("UpdateRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateRole(int roleId, string roleName)
        {
            if (nonManipulatedRoles.Any(x => x.RoleId == roleId && x.RoleName != roleName))
            {
                return BadRequest("it is forbidden to change this role!");
            }

            if (!await _roleRepository.RoleIsExistOfId(roleId))
            {
                return BadRequest("This role cannot exist.");
            }

            if (await _roleRepository.RoleIsExistOfName(roleName))
            {
                return BadRequest("Role with currant name already exist.");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("A role cannot exist without title.");
            }

            if (!await _roleRepository.UpdateRole(new RoleDTO() { RoleId = roleId, RoleName = roleName }))
            {
                ModelState.AddModelError("", "This role doesn't update. No correct data.");
                return StatusCode(400, ModelState);
            }

            return Ok("Operation success");
        }

        [SwaggerOperation(Summary = "Удаление роли")]
        [HttpDelete("DeleteRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            if (!await _roleRepository.RoleIsExistOfId(roleId))
            {
                return BadRequest("This role cannot exist.");
            }

            if (nonManipulatedRoles.Any(x => x.RoleId == roleId))
            {
                return BadRequest("It is forbidden to delete this role!");
            }

            if (!await _roleRepository.DeleteRole(roleId))
            {
                ModelState.AddModelError("", "This role doesn't delete. No correct data.");
                return StatusCode(400, ModelState);
            }

            return Ok("Operation success");
        }
    }
}
