using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Study1CApi.DTOs.MaterialDTOs;

namespace Study1CApi.Interfaces
{
    public interface IMaterialRepository
    {
        public Task<IEnumerable<MaterialTypeDTO>> GetMaterialsTypesAsync();

        public Task<IEnumerable<MaterialDTO>> GetMaterialsAsync(Guid userId, Guid blockId, int materialTypeId);

        public Task<bool> AddMaterialAsync(AddMaterialDTO newMaterial);

        public Task<bool> UpdateMaterialAsync();

        public Task<bool> DeleteChangesAsync(Guid materialId);

        public Task<bool> SaveChangesAsync();
    }
}
