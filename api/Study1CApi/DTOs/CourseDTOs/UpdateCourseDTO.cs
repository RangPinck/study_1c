using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.CourseDTOs
{
    public class UpdateCourseDTO
    {
        [Required(ErrorMessage = "Не указан Id курса!")]
        [Display(Name = "Id курса")]
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "Не указано название курса!")]
        [Display(Name = "Название курса")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Описание курса")]
        public string? Description { get; set; }

        [Display(Name = "Ссылка")]
        public string? Link { get; set; }
    }
}
