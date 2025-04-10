using Study1CApi.DTOs.TaskDTOs;

namespace Study1CApi.Interfaces
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<TaskDTO>> GetTasksOfBlockIdAsync(Guid blockId, Guid userId);

        public Task<IEnumerable<StudyStateDTO>> GetStudyStatesAsync();

        public Task<bool> UpdateTaskStateAsync(Guid taskId, int stateId);

        public Task<bool> UpdateTaskAsync();

        public Task<bool> DeleteTaskAsync(Guid taskId);

        public Task<bool> AddTaskAsync();

        public Task<bool> SaveChangesAsync();
    }
}
