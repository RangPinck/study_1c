using Study1CApi.DTOs.TaskDTOs;

namespace Study1CApi.Interfaces
{
    public interface IStatusStudyRepository
    {
        public Task<IEnumerable<StudyStateDTO>> GetStudyStatesAsync();
    }
}
