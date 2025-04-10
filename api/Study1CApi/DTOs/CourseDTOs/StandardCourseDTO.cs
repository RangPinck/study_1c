namespace Study1CApi.DTOs.CourseDTOs
{
    public class StandardCourseDTO
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public DateTime CourseDataCreate { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public Guid Author { get; set; }
    }
}
