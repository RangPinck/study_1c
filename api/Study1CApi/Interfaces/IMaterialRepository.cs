using Study1CApi.DTOs.MaterialDTOs;

namespace Study1CApi.Interfaces
{
    public interface IMaterialRepository
    {
        public Task<IEnumerable<MaterialTypeDTO>> GetMaterialsTypesAsync();

        public Task<MaterialDTO> GetMaterialByIdAsync(Guid materialId, Guid userId);

        public Task<IEnumerable<MaterialDTO>> GetMaterialsAsync(Guid userId, Guid blockId, int materialTypeId);

        public Task<bool> AddMaterialAsync(AddMaterialDTO newMaterial);

        public Task<bool> UpdateMaterialAsync(UpdateMaterialDTO updateMaterial);

        public Task<bool> DeleteMaterialAsync(Guid materialId);

        public Task<bool> SaveChangesAsync();

        public Task<bool> MaterialComparisonByTitleAndBlockAsync(string title, Guid blockId);

        public Task<Guid?> GetAuthorOfCourseByBlocklIdAsync(Guid blockId);

        public Task<bool> MaterialTypeComparisonByIdAsync(int materialTypeId);

        public Task<MaterialShortDTO> GetMaterialDataByIdAsync(Guid materialId);
    }
}
