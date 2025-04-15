using Study1CApi.DTOs.TaskDTOs;

namespace Study1CApi.Interfaces
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<TaskDTO>> GetTasksOfBlockIdAsync(Guid blockId, Guid userId);

        public Task<bool> UpdateTaskAsync(UpdateTaskDTO updateTask);

        public Task<bool> DeleteTaskAsync(Guid taskId);

        public Task<bool> AddTaskAsync(AddTaskDTO newTask);

        public Task<bool> SaveChangesAsync();

        public Task<TaskDTO> GetTaskByIdAsync(Guid taskId, Guid userId);

        public Task<bool> TaskComparisonByTitleAndBlockAsync(string title, Guid blockId);

        public Task<Guid> GetBlockIdByTaskIdAsync(Guid taskId);
    }
}
