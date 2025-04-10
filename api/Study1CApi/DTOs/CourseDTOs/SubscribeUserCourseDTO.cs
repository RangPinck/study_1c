using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.CourseDTOs
{
    public class SubscribeUserCourseDTO
    {
        [Required(ErrorMessage = "Не указан Id пользователя!")]
        [Display(Name = "Id пользователя")]
        [DefaultValue("")]
        public Guid userId { get; set; }

        [Required(ErrorMessage = "Не указан Id курса!")]
        [Display(Name = "Id курса")]
        [DefaultValue("")]
        public Guid courseId { get; set; }
    }
}
