using Study1CApi.DTOs.TaskDTOs;

namespace Study1CApi.Interfaces
{
    public interface IPracticeRepository
    {
        public Task<IEnumerable<TaskDTO>> GetPracticsOfBlockIdAsync(Guid blockId, Guid userId);

        public Task<bool> UpdatePracticeAsync(UpdateTaskDTO updateTask);

        public Task<bool> DeletePracticeAsync(Guid taskId);

        public Task<bool> AddPracticeAsync(AddTaskDTO newTask);

        public Task<bool> SaveChangesAsync();

        public Task<TaskDTO> GetPracticeByIdAsync(Guid taskId, Guid userId);

        //public Task<bool> TaskComparisonByTitleAndBlockAsync(string title, Guid blockId);

        //public Task<Guid> GetBlockIdByTaskIdAsync(Guid taskId);
    }
}
