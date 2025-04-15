using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.TaskDTOs
{
    public class UpdateTaskDTO
    {
        [Required(ErrorMessage = "Не указан id задачи!")]
        [Display(Name = "TaskId")]
        public Guid TaskId { get; set; }

        [Required(ErrorMessage = "Не указано название задачи!")]
        [Display(Name = "TaskName")]
        public string TaskName { get; set; } = null!;

        [Required(ErrorMessage = "Не указано предполагаемо, затрачиваемое время на решение задачи!")]
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        public string? Link { get; set; }

        public string? Description { get; set; }
    }
}
