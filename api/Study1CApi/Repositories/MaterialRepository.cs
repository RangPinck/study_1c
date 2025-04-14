using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.MaterialDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;

namespace Study1CApi.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly Study1cDbContext _context;

        public MaterialRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaterialTypeDTO>> GetMaterialsTypesAsync()
        {
            return await _context.MaterialTypes.AsNoTracking().Select(material => new MaterialTypeDTO()
            {
                TypeId = material.TypeId,
                TypeName = material.TypeName
            }).ToListAsync();
        }

        public async Task<IEnumerable<MaterialDTO>> GetMaterialsAsync(Guid userId, Guid blockId, int materialTypeId)
        {
            return await _context.BlocksMaterials.AsNoTracking().Where(material => material.Block == blockId && material.MaterialNavigation.Type == materialTypeId).Select(material => new MaterialDTO()
            {
                MaterialId = material.Material,
                MaterialName = material.MaterialNavigation.MaterialName,
                MaterialDateCreate = material.MaterialNavigation.MaterialDateCreate,
                Link = material.MaterialNavigation.Link,
                TypeId = material.MaterialNavigation.Type,
                TypeName = material.MaterialNavigation.TypeNavigation.TypeName,
                Description = material.MaterialNavigation.Description,
                Status = material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).Status != null ? material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).Status : 1,
                DurationMaterial = material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).DurationMaterial != null ? material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).DurationMaterial : 0,
                DateStart = material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).DateStart,
            }).ToListAsync();
        }
    }
}
