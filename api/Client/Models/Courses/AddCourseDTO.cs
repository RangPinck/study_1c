using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Courses
{
    public class AddCourseDTO
    {
        [Required(ErrorMessage = "Не указано название курса!")]
        [Display(Name = "Название курса")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Описание курса")]
        public string? Description { get; set; }

        [Display(Name = "Ссылка")]
        public string? Link { get; set; }

        [Required(ErrorMessage = "Не указан автор курса!")]
        [Display(Name = "Автор курса")]
        public Guid Author { get; set; } = new Guid();
    }
}
