using Study1CApi.DTOs.BlockDTOs;
using Study1CApi.DTOs.CourseDTOs;

namespace Study1CApi.Interfaces
{
    public interface IBlockRepository
    {
        public Task<IEnumerable<ShortBlockDTO>> GetBlocksOfCourseAsync(Guid courseId);

        public Task<bool> AddBlockAsync(AddBlockDTO newBlock);

        public Task<bool> SaveChangesAsync();

        public Task<bool> BlockIsExistByTitleAsync(Guid courseId, string title);

        public Task<bool> BlockIsExistByIdAsync(Guid blockId);

        public Task<StandardCourseDTO> GetCourseByBlockIdAsync(Guid blockId);

        public Task<bool> UpdateBlockAsync(UpdateBlockDTO updateBlock);

        public Task<bool> CompletedTasksFromTheBlockIsExistsAsync(Guid blockId);

        public Task<bool> DeleteBlockAsync(Guid blockId);
    }
}
