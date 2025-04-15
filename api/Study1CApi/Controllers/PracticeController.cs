using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.MaterialDTOs;
using Study1CApi.DTOs.PracticeDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticeController : ControllerBase
    {
        private readonly IPracticeRepository _practiceRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly UserManager<AuthUser> _userManager;

        public PracticeController(IPracticeRepository practiceRepository, ITaskRepository taskRepository, IMaterialRepository materialRepository, UserManager<AuthUser> userManager)
        {
            _practiceRepository = practiceRepository;
            this._taskRepository = taskRepository;
            this._materialRepository = materialRepository;
            _userManager = userManager;
        }

        [SwaggerOperation(Summary = "Получение парктик")]
        [HttpGet("GetPractics")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PracticeDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetPractics(Guid blockId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                var practics = await _practiceRepository.GetPracticsOfBlockIdAsync(blockId, authUser.Id);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(practics);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение практики по Id")]
        [HttpGet("GetPracticeById")]
        [ProducesResponseType(200, Type = typeof(MaterialDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetPracticeById(Guid practiceId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                var material = await _practiceRepository.GetPracticeByIdAsync(practiceId, authUser.Id);

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

        [SwaggerOperation(Summary = "Добавление парктики")]
        [HttpPost("AddPractice")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> AddPractice(AddPracticeDTO newPractice)
        {
            try
            {
                if (await _practiceRepository.PracticeComparisonByTitleAndTaskAsync(newPractice.PracticeName, newPractice.Task))
                {
                    return BadRequest("This practice already exist.");
                }

                if (string.IsNullOrEmpty(newPractice.PracticeName))
                {
                    return BadRequest("A practice cannot exist without title.");
                }

                if (newPractice.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;

                if (await _taskRepository.GetTaskByIdAsync(newPractice.Task, authUser.Id) == null)
                {
                    return BadRequest("This task for practice does not exist!");
                }

                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(await _taskRepository.GetBlockIdByTaskIdAsync(newPractice.Task));
                    if (author != null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _practiceRepository.AddPracticeAsync(newPractice))
                {
                    return BadRequest("This practice doesn't add to database. No correct data.");
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


        [SwaggerOperation(Summary = "Обновление практики")]
        [HttpPut("UpdateMaterial")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> UpdateMaterial(UpdatePracticeDTO updatedPractice)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var practice = await _practiceRepository.GetPracticeByIdAsync(updatedPractice.PracticeId, authUser.Id);

                if (practice == null)
                {
                    return BadRequest("Practice not found!");
                }

                if (string.IsNullOrEmpty(updatedPractice.PracticeName))
                {
                    return BadRequest("A practice cannot exist without title.");
                }

                if (updatedPractice.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }
                
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(await _taskRepository.GetBlockIdByTaskIdAsync(practice.Task));
                    if (author != null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _practiceRepository.UpdatePracticeAsync(updatedPractice))
                {
                    return BadRequest("This practice doesn't update on database. No correct data.");
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


        [SwaggerOperation(Summary = "Удаление практики")]
        [HttpDelete("DeletePractice")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> DeletePractice(Guid practiceId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var practice = await _practiceRepository.GetPracticeByIdAsync(practiceId, authUser.Id);

                if (practice == null)
                {
                    return BadRequest("Practice not found!");
                }

                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(await _taskRepository.GetBlockIdByTaskIdAsync(practice.Task));
                    if (author != null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _practiceRepository.DeletePracticeAsync(practiceId))
                {
                    return BadRequest("This material doesn't delete. No correct data.");
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
