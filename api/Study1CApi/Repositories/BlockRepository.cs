using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.BlockDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;

namespace Study1CApi.Repositories
{
    public class BlockRepository : IBlockRepository
    {
        private readonly Study1cDbContext _context;

        public BlockRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<IEnumerable<ShortBlockDTO>> GetBlocksOfCourseAsync(Guid courseId)
        {
            return await _context.CoursesBlocks.AsNoTracking().Where(x => x.Course == courseId).Select(x => new ShortBlockDTO()
            {
                BlockId = x.BlockId,
                BlockName = x.BlockName,
                BlockDateCreated = x.BlockDateCreated,
                Description = x.Description,
                BlockNumberOfCourse = x.BlockNumberOfCourse
            }).ToListAsync();
        }

        public async Task<bool> AddBlockAsync(AddBlockDTO newBlock)
        {

            var course = await _context.CoursesBlocks.Where(x => x.Course == newBlock.Course).OrderByDescending(x => x.BlockNumberOfCourse).FirstOrDefaultAsync();

            int blockNumber = 1;

            if (course != null)
            {
                blockNumber = (int)course.BlockNumberOfCourse;
            }

            CoursesBlock block = new CoursesBlock()
            {
                BlockName = newBlock.BlockName,
                BlockDateCreated = DateTime.UtcNow,
                Course = newBlock.Course,
                Description = newBlock.Description,
                BlockNumberOfCourse = blockNumber + 1
            };

            await _context.CoursesBlocks.AddAsync(block);

            return await SaveChangesAsync();
        }

        public async Task<bool> BlockIsExistByTitleAsync(Guid courseId, string title)
        {
            return await _context.CoursesBlocks.AnyAsync(x => x.BlockName == title && x.Course == courseId);
        }
    }
}
