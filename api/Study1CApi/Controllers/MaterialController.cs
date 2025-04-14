using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Study1CApi.DTOs.MaterialDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Study1CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly UserManager<AuthUser> _userManager;

        public MaterialController(IMaterialRepository materialRepository, UserManager<AuthUser> userManager)
        {
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

        [SwaggerOperation(Summary = "Получение материала по Id")]
        [HttpGet("GetMaterialById")]
        [ProducesResponseType(200, Type = typeof(MaterialDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetMaterialById(Guid materialId)
        {
            try
            {
                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                var material  = await _materialRepository.GetMaterialByIdAsync(materialId, authUser.Id);

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

        [SwaggerOperation(Summary = "Добавление материала")]
        [HttpPost("AddMaterial")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> AddMaterial(AddMaterialDTO newMaterial)
        {
            try
            {
                if (await _materialRepository.MaterialComparisonByTitleAndBlockAsync(newMaterial.MaterialName, newMaterial.Block))
                {
                    return BadRequest("This material already exist.");
                }

                if (string.IsNullOrEmpty(newMaterial.MaterialName))
                {
                    return BadRequest("A material cannot exist without title.");
                }

                if (newMaterial.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }

                if (!await _materialRepository.MaterialTypeComparisonByIdAsync(newMaterial.TypeId))
                {
                    return BadRequest("This type of material does not exist!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    var author = await _materialRepository.GetAuthorOfCourseByBlocklIdAsync(newMaterial.Block);
                    if (author != null || authUser.Id != author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _materialRepository.AddMaterialAsync(newMaterial))
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

        [SwaggerOperation(Summary = "Обновление материала")]
        [HttpPut("UpdateMaterial")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> UpdateMaterial(UpdateMaterialDTO updatedMaterial)
        {
            try
            {
                var oldMaterial = await _materialRepository.GetMaterialDataByIdAsync(updatedMaterial.MaterialId);

                if (oldMaterial == null)
                {
                    return BadRequest("Material not found!");
                }

                if (string.IsNullOrEmpty(updatedMaterial.MaterialName))
                {
                    return BadRequest("A material cannot exist without title.");
                }

                if (updatedMaterial.Duration <= 0)
                {
                    return BadRequest("The study time cannot be less than or equal to zero!");
                }

                if (!await _materialRepository.MaterialTypeComparisonByIdAsync(updatedMaterial.TypeId))
                {
                    return BadRequest("This type of material does not exist!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != oldMaterial.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _materialRepository.UpdateMaterialAsync(updatedMaterial))
                {
                    return BadRequest("This material doesn't update on database. No correct data.");
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

        [SwaggerOperation(Summary = "Удаление материала")]
        [HttpDelete("DeleteMaterial")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> DeleteMaterial(Guid materialId)
        {
            try
            {
                var material = await _materialRepository.GetMaterialDataByIdAsync(materialId);

                if (material == null)
                {
                    return BadRequest("Material not found!");
                }

                var httpUser = HttpContext.User;
                var authUser = _userManager.FindByEmailAsync(httpUser.Identity.Name).Result;
                var authUserRoles = _userManager.GetRolesAsync(authUser).Result.ToList();

                if (!authUserRoles.Contains("Администратор"))
                {
                    if (authUser.Id != material.Author)
                    {
                        return BadRequest("You don't have enough rights for this operation!");
                    }
                }

                if (!await _materialRepository.DeleteMaterialAsync(materialId))
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
