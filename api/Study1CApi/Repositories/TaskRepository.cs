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

        public async Task<bool> UpdateTaskAsync(UpdateTaskDTO updateTask)
        {
            var task = await _context.BlocksTasks.FirstOrDefaultAsync(x => x.TaskId == updateTask.TaskId);

            task.TaskName = updateTask.TaskName;
            task.Duration = updateTask.Duration;
            task.Link = updateTask.Link;
            task.Description = updateTask.Description;

            _context.BlocksTasks.Update(task);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            var task = await _context.BlocksTasks.FirstOrDefaultAsync(x => x.TaskId == taskId);
            _context.BlocksTasks.Remove(task);
            return await SaveChangesAsync();
        }

        public async Task<bool> AddTaskAsync(AddTaskDTO newTask)
        {
            int numberOfBlock = _context.BlocksTasks.AsNoTracking().Where(x => x.Block == newTask.Block).OrderByDescending(x => x.TaskNumberOfBlock).FirstOrDefault().TaskNumberOfBlock + 1;

            var task = new BlocksTask()
            {
                TaskName = newTask.TaskName,
                TaskDateCreated = DateTime.UtcNow,
                Duration = newTask.Duration,
                Block = newTask.Block,
                Link = newTask.Link,
                TaskNumberOfBlock = numberOfBlock,
                Description = newTask.Description
            };

            await _context.BlocksTasks.AddAsync(task);

            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<TaskDTO> GetTaskByIdAsync(Guid taskId, Guid userId)
        {
            return await _context.BlocksTasks.AsNoTracking().Select(task => new TaskDTO()
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

            }).FirstOrDefaultAsync(x => x.TaskId == taskId);
        }

        public async Task<bool> TaskComparisonByTitleAndBlockAsync(string title, Guid blockId)
        {
            return await _context.BlocksTasks.AnyAsync(x => x.Block == blockId && title == x.TaskName);
        }

        public async Task<Guid> GetBlockIdByTaskIdAsync(Guid taskId)
        {
            return _context.BlocksTasks.FirstOrDefault(x => x.TaskId == taskId).Block;
        }
    }
}
