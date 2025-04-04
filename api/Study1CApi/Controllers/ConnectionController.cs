using Microsoft.AspNetCore.Mvc;
using Study1CApi.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly IConnectionRepository _connectionRepository;

        public ConnectionController(IConnectionRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        [SwaggerOperation(Summary = "Проверка подключения к базе данных")]
        [HttpGet("CheckConnection")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CheckConnection()
        {
            var connection = await _connectionRepository.CheckConnectionAsync();

            if (connection.IsConnect == ConnectionEnum.Connect){
                return Ok("Database is connected!");
            }else{
                return BadRequest(connection.Error);
            }
        }
    }
}
