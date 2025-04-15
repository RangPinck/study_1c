using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.PracticeDTOs;
using Study1CApi.DTOs.TaskDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using System.Diagnostics;

namespace Study1CApi.Repositories
{
    public class PracticeRepository : IPracticeRepository
    {
        private readonly Study1cDbContext _context;

        public PracticeRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPracticeAsync(AddPracticeDTO newPractice)
        {
            var lastPracticeTask = await _context.TasksPractices.AsNoTracking().OrderByDescending(x => x.NumberPracticeOfTask).FirstOrDefaultAsync();

            int numberPractice = 1;

            if (lastPracticeTask != null)
            {
                numberPractice = (int)lastPracticeTask.NumberPracticeOfTask + 1;
            }

            var practice = new TasksPractice()
            {
                PracticeName = newPractice.PracticeName,
                PracticeDateCreated = DateTime.UtcNow,
                Duration = newPractice.Duration,
                Link = newPractice.Link,
                Task = newPractice.Task,
                NumberPracticeOfTask = numberPractice
            };

            await _context.TasksPractices.AddAsync(practice);

            return await SaveChangesAsync();
        }

        public async Task<bool> UpdatePracticeAsync(UpdatePracticeDTO updatePractice)
        {
            var practice = await _context.TasksPractices.FirstOrDefaultAsync(x => x.PracticeId == updatePractice.PracticeId);

            practice.PracticeName = updatePractice.PracticeName;
            practice.Duration = updatePractice.Duration;
            practice.Link = updatePractice.Link;

            _context.TasksPractices.Update(practice);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeletePracticeAsync(Guid practiceId)
        {
            var practice = await _context.TasksPractices.FirstOrDefaultAsync(x => x.PracticeId == practiceId);
            _context.TasksPractices.Remove(practice);
            return await SaveChangesAsync();
        }

        public async Task<PracticeDTO> GetPracticeByIdAsync(Guid practiceId, Guid userId)
        {
            return await _context.TasksPractices.AsNoTracking().Select(practice => new PracticeDTO()
            {
                PracticeId = practice.PracticeId,
                PracticeName = practice.PracticeName,
                PracticeDateCreated = practice.PracticeDateCreated,
                DurationNeeded = practice.Duration,
                Link = practice.Link,
                Task = practice.Task,
                NumberPracticeOfTask = practice.NumberPracticeOfTask,
                Status = practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practiceId) != null ? practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practiceId).Status : 1,
                DateStart = practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practiceId).DateStart,
                Duration = practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practiceId) != null ? practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practiceId).DurationPractice : 0,
            }).FirstOrDefaultAsync(x => x.PracticeId == practiceId);
        }

        public async Task<IEnumerable<PracticeDTO>> GetPracticsOfBlockIdAsync(Guid blockId, Guid userId)
        {
            List<Guid> tasks = await _context.BlocksTasks.Where(x => x.Block == blockId).Select(x => x.TaskId).ToListAsync();

            return await _context.TasksPractices.AsNoTracking().Where(x => tasks.Contains(x.Task)).Select(practice => new PracticeDTO()
            {
                PracticeId = practice.PracticeId,
                PracticeName = practice.PracticeName,
                PracticeDateCreated = practice.PracticeDateCreated,
                DurationNeeded = practice.Duration,
                Link = practice.Link,
                Task = practice.Task,
                NumberPracticeOfTask = practice.NumberPracticeOfTask,
                Status = practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practice.PracticeId) != null ? practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practice.PracticeId).Status : 1,
                DateStart = practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practice.PracticeId).DateStart,
                Duration = practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practice.PracticeId) != null ? practice.UsersTasks.FirstOrDefault(x => x.AuthUser == userId && x.Practice == practice.PracticeId).DurationPractice : 0
            }).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        public async Task<bool> PracticeComparisonByTitleAndTaskAsync(string title, Guid taskId)
        {
            return await _context.TasksPractices.AnyAsync(x => x.Task == taskId && title == x.PracticeName);
        }
    }
}
