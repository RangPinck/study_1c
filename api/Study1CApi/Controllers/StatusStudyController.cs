using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.TaskDTOs;
using Study1CApi.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusStudyController : ControllerBase
    {
        private readonly IStatusStudyRepository _statusStudyRepository;

        public StatusStudyController(IStatusStudyRepository statusStudyRepository)
        {
            this._statusStudyRepository = statusStudyRepository;
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
    }
}
