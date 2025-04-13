using Study1CApi.DTOs.CourseDTOs;

namespace Study1CApi.Interfaces
{
    public interface ICourseRepository
    {
        public Task<IEnumerable<ShortCourseDTO>> GetAllCoursesAsync();

        public Task<FullCourseDTO> GetCourseByIdAsync(Guid courseId);

        public Task<bool> AddCourseAsync(AddCourseDTO newCourse);

        public Task<bool> UpdateCourseAsync(UpdateCourseDTO updatedCourse);

        public Task<bool> SaveChangesAsync();

        public Task<bool> CourseComparisonByAuthorAndTitleAsync(Guid authorId, string courseTitle);

        public Task<StandardCourseDTO> GetCourseDataByIdAsync(Guid courseId);

        public Task<bool> DeleteCourseAsync(Guid courseId);

        public Task<bool> CheckSubsOnCourseAsync(Guid courseId);

        public Task<bool> SubscribeUserForACourseAsync(SubscribeUserCourseDTO  suc);

        public Task<bool> UnsubscribeUserForACourseAsync(SubscribeUserCourseDTO suc);

        public Task<bool> CheckUserSubscribeOnCourseAsync(SubscribeUserCourseDTO suc);

        public Task<IEnumerable<ShortCourseDTO>> GetCoursesThatUserSubscribeAsync(Guid userId);

        public Task<IEnumerable<ShortCourseDTO>> GetCoursesThatUserCreateAsync(Guid authorId);

        public Task<bool> CourseIsExistByIdAsync(Guid courseId);
    }
}
