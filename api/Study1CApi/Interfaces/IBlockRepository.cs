using Study1CApi.DTOs.BlockDTOs;

namespace Study1CApi.Interfaces
{
    public interface IBlockRepository
    {
        public Task<IEnumerable<ShortBlockDTO>> GetBlocksOfCourseAsync(Guid courseId);

        public Task<bool> AddBlockAsync(AddBlockDTO newBlock);

        public Task<bool> SaveChangesAsync();

        public Task<bool> BlockIsExistByTitleAsync(Guid courseId, string title);
    }
}
