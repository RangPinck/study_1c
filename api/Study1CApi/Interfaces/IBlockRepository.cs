using Study1CApi.DTOs.BlockDTOs;

namespace Study1CApi.Interfaces
{
    public interface IBlockRepository
    {
        public Task<IEnumerable<ShortBlockDTO>> GetBlocksOfCourseAsync(Guid courseId);

        public Task<bool> AddBlock(AddBlockDTO newBlock);

        public Task<bool> SaveChangesAsync();
    }
}
