using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.StudyStateDTOs;
using Study1CApi.DTOs.TaskDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusStudyController : ControllerBase
    {
        private readonly IStatusStudyRepository _statusStudyRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly UserManager<AuthUser> _userManager;

        public StatusStudyController(IStatusStudyRepository statusStudyRepository, IMaterialRepository materialRepository, UserManager<AuthUser> userManager)
        {
            _statusStudyRepository = statusStudyRepository;
            _materialRepository = materialRepository;
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
                var studyStates = await _statusStudyRepository.GetStudyStatesAsync();

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

        [SwaggerOperation(Summary = "Обновление статуса изучения")]
        [HttpPost("UpdateStudyState")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> UpdateStudyState(UpdateStudyStateDTO updatedState)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                bool taskExist = !await _statusStudyRepository.CheckTaskByIdAsync(updatedState.UpdateObjectId);
                bool masterialExist = !await _statusStudyRepository.CheckMaterialByIdAsync(updatedState.UpdateObjectId);
                bool practiceExist = !await _statusStudyRepository.CheckPracticeByIdAsync(updatedState.UpdateObjectId);

                if (taskExist && masterialExist && practiceExist)
                {
                    return BadRequest("This object not found.");
                }

                if (updatedState.Duration < 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(updatedState.BlockId);
                    if (author != null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _statusStudyRepository.UpdateStateAsync(updatedState, authUser.Id))
                {
                    return BadRequest("This state doesn't update on database. No correct data.");
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
