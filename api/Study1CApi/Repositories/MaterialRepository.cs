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
                Duration = material.Duration,
                Note = material.Note,
                BmDateCreate = material.BmDateCreate,
                Status = material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).Status != null ? material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).Status : 1,
                DurationNeeded = material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).DurationMaterial != null ? material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).DurationMaterial : 0,
                DateStart = material.UsersTasks.FirstOrDefault(user => user.AuthUser == userId).DateStart,
            }).ToListAsync();
        }

        public async Task<bool> AddMaterialAsync(AddMaterialDTO newMaterial)
        {
            var material = new Material()
            {
                MaterialName = newMaterial.MaterialName,
                MaterialDateCreate = DateTime.UtcNow,
                Link = newMaterial.Link,
                Type = newMaterial.TypeId,
                Description = newMaterial.Description,
                BlocksMaterials = new List<BlocksMaterial>(){ new BlocksMaterial()
                {
                    Duration = newMaterial.Duration,
                    Block = newMaterial.Block,
                    Note = newMaterial.Note
                }}
            };

            _context.Materials.Attach(material);
            _context.Materials.Add(material);

            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateMaterialAsync(UpdateMaterialDTO updateMaterial)
        {
            var material = await _context.BlocksMaterials.Include(x => x.MaterialNavigation).FirstOrDefaultAsync(material => material.MaterialNavigation.MaterialId == updateMaterial.MaterialId);

            material.MaterialNavigation.MaterialName = updateMaterial.MaterialName;
            material.MaterialNavigation.Link = updateMaterial.Link;
            material.MaterialNavigation.Type = updateMaterial.TypeId;
            material.MaterialNavigation.Description = updateMaterial.Description;
            material.Duration = updateMaterial.Duration;
            material.Note = updateMaterial.Note;

            _context.BlocksMaterials.Attach(material);
            _context.BlocksMaterials.Update(material);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteMaterialAsync(Guid materialId)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(x => x.MaterialId == materialId);

            _context.Materials.Remove(material);

            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<bool> MaterialComparisonByTitleAndBlockAsync(string title, Guid blockId)
        {
            return await _context.BlocksMaterials.AsNoTracking().AnyAsync(x => x.MaterialNavigation.MaterialName == title && x.Block == blockId);
        }

        public async Task<Guid?> GetAuthorOfCourseByBlocklIdAsync(Guid blockId)
        {
            var block = await _context.CoursesBlocks.AsNoTracking().Include(x => x.CourseNavigation).FirstOrDefaultAsync(x => x.BlockId == blockId);

            return block != null ? block.CourseNavigation.Author : null;
        }

        public async Task<bool> MaterialTypeComparisonById(int materialTypeId)
        {
            return await _context.MaterialTypes.AsNoTracking().AnyAsync(x => x.TypeId == materialTypeId);
        }

        public async Task<MaterialShortDTO> GetMaterialDataById(Guid materialId)
        {
            return await _context.BlocksMaterials.AsNoTracking().Select(material => new MaterialShortDTO()
            {
                MaterialId = material.MaterialNavigation.MaterialId,
                Author = material.BlockNavigation.CourseNavigation.Author,
            }).FirstOrDefaultAsync(x => x.MaterialId == materialId);
        }
    }
}
