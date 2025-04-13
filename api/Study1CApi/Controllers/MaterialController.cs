using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.MaterialDTOs;
using Study1CApi.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;
        public MaterialController(IMaterialRepository materialRepository)
        {
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
    }
}
