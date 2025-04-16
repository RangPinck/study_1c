using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.DTOs.UserDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Study1CApi.DTOs.BlockDTOs;

namespace Study1CApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Study1cDbContext _context;
        private readonly UserManager<AuthUser> _userManager;

        public UserRepository(Study1cDbContext context, UserManager<AuthUser> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ICollection<UserDTO>> GetAllUsersAsync(bool isAdmin = false)
        {
            try
            {
                var users = await _userManager.Users.AsNoTracking().ToListAsync();
                var profiles = await _context.Users.Include(x => x.UserCourses).Include(x => x.UsersTasks).AsNoTracking().ToListAsync();

                List<UserDTO> result = new List<UserDTO>();

                if (isAdmin)
                {
                    result = users.Select(user => new UserDTO()
                    {
                        UserId = user.Id,
                        UserLogin = user.Email,
                        UserSurname = profiles.FirstOrDefault(x => x.UserId == user.Id).UserSurname,
                        UserName = profiles.FirstOrDefault(x => x.UserId == user.Id).UserName,
                        UserPatronymic = profiles.FirstOrDefault(x => x.UserId == user.Id).UserPatronymic,
                        UserDataCreate = user.UserDataCreate,
                        UserRole = _userManager.GetRolesAsync(user).Result.ToList(),
                        UserStatistics = _context.CoursesBlocks.Select(block => new BlockStatisticsDTO()
                        {
                            BlockId = block.BlockId,

                            FullyCountTask = block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum(),

                            FullyDurationNeeded = block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum(),

                            CompletedTaskCount = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count(),

                            DurationCompletedTask = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask),

                            PercentCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count() / (double)(block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum()) * 100.0, 2),

                            PercentDurationCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask) / (double)(block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum()) * 100.0, 2),
                        }).ToList()
                    }).ToList();
                }
                else
                {
                    result = users.Select(user => new UserDTO()
                    {
                        UserId = user.Id,
                        UserLogin = user.Email,
                        UserSurname = profiles.FirstOrDefault(x => x.UserId == user.Id).UserSurname,
                        UserName = profiles.FirstOrDefault(x => x.UserId == user.Id).UserName,
                        UserPatronymic = profiles.FirstOrDefault(x => x.UserId == user.Id).UserPatronymic,
                        UserDataCreate = user.UserDataCreate,
                        UserRole = _userManager.GetRolesAsync(user).Result.ToList(),
                        UserStatistics = _context.CoursesBlocks.Select(block => new BlockStatisticsDTO()
                        {
                            BlockId = block.BlockId,

                            FullyCountTask = block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum(),

                            FullyDurationNeeded = block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum(),

                            CompletedTaskCount = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count(),

                            DurationCompletedTask = _context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask),

                            PercentCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Count() / (double)(block.BlocksTasks.Count() + block.BlocksMaterials.Where(mat => mat.Duration != null).Count() + block.BlocksTasks.Select(x => x.TasksPractices.Count()).Sum()) * 100.0, 2),

                            PercentDurationCompletedTask = Math.Round((double)_context.UsersTasks.Where(x => x.AuthUser == user.Id && x.Status == 3 && (x.MaterialNavigation.Block == block.BlockId || x.TaskNavigation.Block == block.BlockId || x.PracticeNavigation.TaskNavigation.Block == block.BlockId)).Sum(x => x.DurationMaterial + x.DurationPractice + x.DurationTask) / (double)(block.BlocksTasks.Select(x => x.Duration).Sum() + block.BlocksMaterials.Where(mat => mat.Duration != null).Select(x => Convert.ToInt32(x.Duration)).Sum() + block.BlocksTasks.Select(x => x.TasksPractices.Select(x => Convert.ToInt32(x.Duration)).Sum()).Sum()) * 100.0, 2),
                        }).ToList()
                    }).Where(x => x.UserRole.Contains("Ученик")).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<UserDTO>();
            }
        }

        public async Task<bool> UserIsExistAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<CourseAuthorDTO>> GetAuthorsForCoursesAsync()
        {
            List<Guid> users = _userManager.GetUsersInRoleAsync("Администратор").Result.Select(x => x.Id).ToList();
            users = [.. users, .. _userManager.GetUsersInRoleAsync("Куратор").Result.Select(x => x.Id).Distinct().ToList()];

            return await _context.Users.AsNoTracking().Where(x => users.Contains(x.UserId)).Select(x => new CourseAuthorDTO()
            {
                UserId = x.UserId,
                UserName = x.UserName,
                UserSurname = x.UserSurname,
                UserPatronymic = x.UserPatronymic
            }).ToListAsync();
        }
    }
}
