using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.MaterialDTOs
{
    public class MaterialToBlockDTO
    {
        [Required(ErrorMessage = "Не указан id блока!")]
        [Display(Name = "Block")]
        public Guid Block { get; set; }

        [Required(ErrorMessage = "Не указан id материала!")]
        [Display(Name = "Material")]
        public Guid Material { get; set; }

        public string? Note { get; set; }

        public int? Duration { get; set; }
    }
}
