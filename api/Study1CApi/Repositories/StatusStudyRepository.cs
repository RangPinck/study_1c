using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.StudyStateDTOs;
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

        public async Task<bool> CheckMaterialByIdAsync(Guid materialId)
        {
            return await _context.BlocksMaterials.AsNoTracking().AnyAsync(x => x.BmId == materialId);
        }

        public async Task<bool> CheckPracticeByIdAsync(Guid practiceId)
        {
            return await _context.TasksPractices.AsNoTracking().AnyAsync(x => x.PracticeId == practiceId);
        }

        public async Task<bool> CheckTaskByIdAsync(Guid taskId)
        {
            return await _context.BlocksTasks.AsNoTracking().AnyAsync(x => x.TaskId == taskId);
        }

        public async Task<IEnumerable<StudyStateDTO>> GetStudyStatesAsync()
        {
            return await _context.StudyStates.AsNoTracking().Select(x => new StudyStateDTO()
            {
                StateId = x.StateId,
                StateName = x.StateName
            }).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<bool> UpdateStateAsync(UpdateStudyStateDTO state, Guid userId)
        {
            var task = await _context.BlocksTasks.AsNoTracking().FirstOrDefaultAsync(x => x.TaskId == state.UpdateObjectId);
            var practice = await _context.TasksPractices.AsNoTracking().FirstOrDefaultAsync(x => x.PracticeId == state.UpdateObjectId);
            var material = await _context.BlocksMaterials.AsNoTracking().FirstOrDefaultAsync(x => x.BmId == state.UpdateObjectId);

            var tableRow = await _context.UsersTasks.AsNoTracking().FirstOrDefaultAsync(x => x.AuthUser == userId && ((x.Task != null && x.Task == state.UpdateObjectId) || (x.Practice != null && x.Practice == state.UpdateObjectId) || (x.Material != null && x.Material == state.UpdateObjectId)));

            switch (state.StateId)
            {
                case 1:
                    _context.UsersTasks.Remove(tableRow);
                    break;
                case 2:
                    if (tableRow == null)
                    {
                        await _context.UsersTasks.AddAsync(GenerateRegistrationModel(task, practice, material, userId));
                    }
                    else
                    {
                        tableRow.Practice = practice != null ? practice.PracticeId : null;
                        tableRow.Material = material != null ? material.BmId : null;
                        tableRow.Task = task != null ? task.TaskId : null;
                        tableRow.Status = 2;
                        tableRow.DateStart = DateTime.UtcNow;
                        _context.UsersTasks.Update(tableRow);
                    }
                    break;
                case 3:
                    if (tableRow == null)
                    {
                        var newState = GenerateRegistrationModel(task, practice, material, userId);
                        newState.DurationMaterial = material != null ? state.Duration : 0;
                        newState.DurationTask = task != null ? state.Duration : 0;
                        newState.Status = 3;
                        newState.DurationPractice = practice != null ? state.Duration : 0;
                        await _context.UsersTasks.AddAsync(newState);
                    }
                    else
                    {
                        tableRow.Practice = practice != null ? practice.PracticeId : null;
                        tableRow.Material = material != null ? material.BmId : null;
                        tableRow.Task = task != null ? task.TaskId : null;
                        tableRow.Status = 3;
                        tableRow.DurationMaterial = material != null ? state.Duration : 0;
                        tableRow.DurationTask = task != null ? state.Duration : 0;
                        tableRow.DurationPractice = practice != null ? state.Duration : 0;
                        tableRow.DateStart = DateTime.UtcNow;
                        _context.UsersTasks.Update(tableRow);
                    }
                    break;
            }

            return await SaveChangesAsync();
        }

        private UsersTask GenerateRegistrationModel(BlocksTask? task, TasksPractice? practice, BlocksMaterial? material, Guid userId)
        {
            return new UsersTask()
            {
                Practice = practice != null ? practice.PracticeId : null,
                Material = material != null ? material.BmId : null,
                Task = task != null ? task.TaskId : null,
                Status = 2,
                DateStart = DateTime.UtcNow,
                AuthUser = userId
            };
        }
    }
}
