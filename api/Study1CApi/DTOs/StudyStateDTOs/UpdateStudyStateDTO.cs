using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.StudyStateDTOs
{
    public class UpdateStudyStateDTO
    {
        [Required(ErrorMessage = "Не указан id присваемого статуса!")]
        [Display(Name = "StateId")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "Не указан id задачи, практики или материала (blockMaterial)")]
        [Display(Name = "UpdateObjectId")]
        public Guid UpdateObjectId { get; set; }

        [Required(ErrorMessage = "Не указан id блока!")]
        [Display(Name = "BlockId")]
        public Guid BlockId { get; set; }

        [Display(Name = "Duration")]
        public int Duration { get; set; }
    }
}
