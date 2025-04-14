using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.MaterialDTOs;
using Study1CApi.DTOs.TaskDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockDataController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<AuthUser> _userManager;

        public BlockDataController(IMaterialRepository materialRepository, ITaskRepository taskRepository, UserManager<AuthUser> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
            _materialRepository = materialRepository;
        }

        [SwaggerOperation(Summary = "Получение типов материалов")]
        [HttpGet("GetMaterialsTypes")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MaterialTypeDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> GetMaterialsTypes()
        {
            try
            {
                IEnumerable<MaterialTypeDTO> materialsTypes = await _materialRepository.GetMaterialsTypesAsync();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(materialsTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение материалов")]
        [HttpGet("GetMaterials")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MaterialDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetMaterials(Guid blockId, int materialTypeId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                IEnumerable<MaterialDTO> materials = await _materialRepository.GetMaterialsAsync(authUser.Id, blockId, materialTypeId);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(materials);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение статусов задач")]
        [HttpGet("GetStudyStatesAsync")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StudyStateDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetStudyStatesAsync()
        {
            try
            {
                var studyStates = await _taskRepository.GetStudyStatesAsync();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(studyStates);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение задач")]
        [HttpGet("GetTasksOfBlockIdAsync")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaskDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetTasksOfBlockIdAsync(Guid blockId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                var tasks = await _taskRepository.GetTasksOfBlockIdAsync(blockId, authUser.Id);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }
    }
}
