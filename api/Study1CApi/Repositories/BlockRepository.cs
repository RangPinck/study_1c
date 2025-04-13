using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.BlockDTOs;
using Study1CApi.DTOs.CourseDTOs;
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

            int blockNumber = 0;

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

        public async Task<bool> BlockIsExistByIdAsync(Guid blockId)
        {
            return await _context.CoursesBlocks.AnyAsync(x => x.BlockId == blockId);
        }

        public async Task<StandardCourseDTO> GetCourseByBlockIdAsync(Guid blockId)
        {
            var block = await _context.CoursesBlocks.AsNoTracking().FirstOrDefaultAsync(x => x.BlockId == blockId);

            return await _context.Courses.AsNoTracking().Select(x => new StandardCourseDTO()
            {
                CourseId = x.CourseId,
                CourseName = x.CourseName,
                CourseDataCreate = x.CourseDataCreate,
                Description = x.Description,
                Link = x.Link,
                Author = x.Author
            }).FirstOrDefaultAsync(x => x.CourseId == block.Course);
        }

        public async Task<bool> UpdateBlockAsync(UpdateBlockDTO updateBlock)
        {
            var block = await _context.CoursesBlocks.FirstOrDefaultAsync(x => x.BlockId == updateBlock.BlockId);

            block.BlockName = updateBlock.BlockName;
            block.Description = updateBlock.Description;

            _context.CoursesBlocks.Update(block);

            return await SaveChangesAsync();
        }

        public async Task<bool> CompletedTasksFromTheBlockIsExistsAsync(Guid blockId)
        {
            var listTasksBlock = await _context.BlocksTasks.AsNoTracking().Where(x => x.Block == blockId).Select(x => x.TaskId).ToListAsync();

            if (listTasksBlock.Count == 0)
            {
                return false;
            }

            return await _context.UsersTasks.AnyAsync(x => listTasksBlock.Contains((Guid)x.Task));
        }

        public async Task<bool> DeleteBlockAsync(Guid blockId)
        {
            var block = await _context.CoursesBlocks.FirstOrDefaultAsync(x => x.BlockId == blockId);
            _context.CoursesBlocks.Remove(block);
            return await SaveChangesAsync();
        }
    }
}
