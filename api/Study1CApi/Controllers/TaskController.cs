using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.BlockDTOs;
using Study1CApi.DTOs.CourseDTOs;
using Study1CApi.DTOs.TaskDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        // private readonly ITaskRepository _taskRepository;
        // private readonly UserManager<AuthUser> _userManager;

        // public TaskController(ITaskRepository taskRepository, UserManager<AuthUser> userManager)
        // {
        //     _taskRepository = taskRepository;
        //     _userManager = userManager;
        // }

        // [SwaggerOperation(Summary = "Получение статусов задач")]
        // [HttpGet("GetStudyStatesAsync")]
        // [ProducesResponseType(200, Type = typeof(IEnumerable<StudyStateDTO>))]
        // [ProducesResponseType(400)]
        // [ProducesResponseType(500)]
        // [Authorize]
        // public async Task<IActionResult> GetStudyStatesAsync()
        // {
        //     try
        //     {
        //         var studyStates = await _taskRepository.GetStudyStatesAsync();

        //         if (!ModelState.IsValid)
        //         {
        //             return BadRequest();
        //         }

        //         return Ok(studyStates);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(503, ex.Message);
        //     }
        // }

        // [SwaggerOperation(Summary = "Получение задач")]
        // [HttpGet("GetTasksOfBlockIdAsync")]
        // [ProducesResponseType(200, Type = typeof(IEnumerable<TaskDTO>))]
        // [ProducesResponseType(400)]
        // [ProducesResponseType(500)]
        // [Authorize]
        // public async Task<IActionResult> GetTasksOfBlockIdAsync(Guid blockId)
        // {
        //     try
        //     {
        //         var httpUser = HttpContext.User;
        //         var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
        //         var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

        //         var tasks = await _taskRepository.GetTasksOfBlockIdAsync(blockId, authUser.Id);

        //         if (!ModelState.IsValid)
        //         {
        //             return BadRequest();
        //         }

        //         return Ok(tasks);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(503, ex.Message);
        //     }
        // }
    }
}
