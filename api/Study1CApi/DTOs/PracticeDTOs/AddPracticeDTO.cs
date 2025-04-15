using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.PracticeDTOs
{
    public class AddPracticeDTO
    {
        [Required(ErrorMessage = "Не указано название практики!")]
        [Display(Name = "PracticeName")]
        public string PracticeName { get; set; } = null!;

        [Required(ErrorMessage = "Не указана длительность выполнения практической части задания!")]
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Display(Name = "Link")]
        public string? Link { get; set; }

        [Required(ErrorMessage = "Не указан id задачи!")]
        [Display(Name = "Task")]
        public Guid Task { get; set; }
    }
}
