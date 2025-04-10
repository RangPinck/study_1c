using Study1CApi.DTOs.CourseDTOs;

namespace Study1CApi.Interfaces
{
    public interface ICourseRepository
    {
        public Task<IEnumerable<ShortCourseDTO>> GetAllCourses();

        public Task<FullCourseDTO> GetCourseById(Guid courseId);

        public Task<bool> AddCourse(AddCourseDTO newCourse);

        public Task<bool> UpdateCourse(UpdateCourseDTO updatedCourse);

        public Task<bool> SaveChangesAsync();

        public Task<bool> CourseComparisonByAuthorAndTitle(Guid authorId, string courseTitle);

        public Task<StandardCourseDTO> GetCourseDataById(Guid courseId);

        public Task<bool> DeleteCourse(Guid courseId);

        public Task<bool> CheckSubsOnCourse(Guid courseId);

        public Task<bool> SubscribeUserForACourse(SubscribeUserCourseDTO  suc);

        public Task<bool> UnsubscribeUserForACourse(SubscribeUserCourseDTO suc);

        public Task<bool> CheckUserSubscribeOnCourse(SubscribeUserCourseDTO suc);

        public Task<IEnumerable<ShortCourseDTO>> GetCoursesThatUserSubscribe(Guid userId);

        public Task<IEnumerable<ShortCourseDTO>> GetCoursesThatUserCreate(Guid authorId);
    }
}
