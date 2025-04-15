using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.MaterialDTOs;
using Study1CApi.DTOs.TaskDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly UserManager<AuthUser> _userManager;

        public TaskController(IMaterialRepository materialRepository, ITaskRepository taskRepository, IBlockRepository blockRepository, UserManager<AuthUser> userManager)
        {
            _materialRepository = materialRepository;
            _taskRepository = taskRepository;
            _blockRepository = blockRepository;
            _userManager = userManager;
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
        public async Task<IActionResult> GetTasksOfBlockId(Guid blockId)
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

        [SwaggerOperation(Summary = "Получение задачи по Id")]
        [HttpGet("GetTaskById")]
        [ProducesResponseType(200, Type = typeof(MaterialDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetTaskById(Guid taskId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                var material = await _taskRepository.GetTaskByIdAsync(taskId, authUser.Id);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(material);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Добавление задачи")]
        [HttpPost("AddTask")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> AddTask(AddTaskDTO newTask)
        {
            try
            {
                if (!await _blockRepository.BlockIsExistByIdAsync(newTask.Block))
                {
                    return BadRequest("This block not found.");
                }

                if (string.IsNullOrEmpty(newTask.TaskName))
                {
                    return BadRequest("A task cannot exist without title.");
                }

                if (await _taskRepository.TaskComparisonByTitleAndBlockAsync(newTask.TaskName, newTask.Block))
                {
                    return BadRequest("This task already exist.");
                }

                if (newTask.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(newTask.Block);
                    if (author != null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _taskRepository.AddTaskAsync(newTask))
                {
                    return BadRequest("This material doesn't add to database. No correct data.");
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

        [SwaggerOperation(Summary = "Обновление задачи")]
        [HttpPut("UpdateTask")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> UpdateTask(UpdateTaskDTO updatedTask)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();
                var task = await _taskRepository.GetTaskByIdAsync(updatedTask.TaskId, authUser.Id);

                if (task == null)
                {
                    return BadRequest("This task not found.");
                }

                if (string.IsNullOrEmpty(updatedTask.TaskName))
                {
                    return BadRequest("A material cannot exist without title.");
                }

                if (updatedTask.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }


                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(await _taskRepository.GetBlockIdByTaskIdAsync(updatedTask.TaskId));
                    if (author != null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _taskRepository.UpdateTaskAsync(updatedTask))
                {
                    return BadRequest("This task doesn't update on database. No correct data.");
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

        [SwaggerOperation(Summary = "Удаление задачи")]
        [HttpDelete("DeleteTask")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();
                var task = await _taskRepository.GetTaskByIdAsync(taskId, authUser.Id);

                if (task == null)
                {
                    return BadRequest("This task not found.");
                }

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(await _taskRepository.GetBlockIdByTaskIdAsync(taskId));
                    if (author != null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _taskRepository.DeleteTaskAsync(taskId))
                {
                    return BadRequest("This task doesn't delete. No correct data.");
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
