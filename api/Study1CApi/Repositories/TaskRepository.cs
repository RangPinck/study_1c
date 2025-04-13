using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.TaskDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;

namespace Study1CApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly Study1cDbContext _context;

        public TaskRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskDTO>> GetTasksOfBlockIdAsync(Guid blockId, Guid userId)
        {
            return await _context.BlocksTasks.AsNoTracking().Where(x => x.Block == blockId).Select(x => new TaskDTO()
            {
                TaskId = x.TaskId,
                TaskName = x.TaskName,
                TaskDateCreated = x.TaskDateCreated,
                Duration = x.Duration,
                Link = x.Link,
                TaskNumberOfBlock = x.TaskNumberOfBlock,
                Description = x.Description
            }).ToListAsync();
        }

        public async Task<IEnumerable<StudyStateDTO>> GetStudyStatesAsync()
        {
            return await _context.StudyStates.AsNoTracking().Select(x => new StudyStateDTO()
            {
                StateId = x.StateId,
                StateName = x.StateName
            }).ToListAsync();
        }

        public async Task<bool> UpdateTaskStateAsync(Guid taskId, int stateId)
        {
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateTaskAsync()
        {
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            return await SaveChangesAsync();
        }

        public async Task<bool> AddTaskAsync()
        {
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }
    }
}
