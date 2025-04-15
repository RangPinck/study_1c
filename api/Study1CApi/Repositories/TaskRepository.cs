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
            return await _context.BlocksTasks.AsNoTracking().Where(task => task.Block == blockId).Select(task => new TaskDTO()
            {
                TaskId = task.TaskId,
                TaskName = task.TaskName,
                TaskDateCreated = task.TaskDateCreated,
                DurationNeeded = task.Duration,
                Link = task.Link,
                TaskNumberOfBlock = task.TaskNumberOfBlock,
                Description = task.Description,
                Status = task.UsersTasks.FirstOrDefault(ut => userId == ut.AuthUser && ut.Task == task.TaskId) != null ? task.UsersTasks.FirstOrDefault(ut => userId == ut.AuthUser && ut.Task == task.TaskId).Status : 1,
                DateStart = task.UsersTasks.FirstOrDefault(ut => userId == ut.AuthUser && ut.Task == task.TaskId).DateStart,
                Duration = task.UsersTasks.FirstOrDefault(ut => userId == ut.AuthUser && ut.Task == task.TaskId) != null ? task.UsersTasks.FirstOrDefault(ut => userId == ut.AuthUser && ut.Task == task.TaskId).DurationTask : 0,

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
