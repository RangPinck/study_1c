using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.TaskDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;

namespace Study1CApi.Repositories
{
    public class StatusStudyRepository : IStatusStudyRepository
    {
        private readonly Study1cDbContext _context;

        public StatusStudyRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudyStateDTO>> GetStudyStatesAsync()
        {
            return await _context.StudyStates.AsNoTracking().Select(x => new StudyStateDTO()
            {
                StateId = x.StateId,
                StateName = x.StateName
            }).ToListAsync();
        }
    }
}
