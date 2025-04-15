using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Study1CApi.DTOs.MaterialDTOs
{
    public class UpdateMaterialDTO
    {
        [Required(ErrorMessage = "Не указан id материала!")]
        [Display(Name = "MaterialId")]
        public Guid MaterialId { get; set; }

        [Required(ErrorMessage = "Не указано название материала!")]
        [Display(Name = "MaterialName")]
        public string MaterialName { get; set; } = null!;

        [Display(Name = "Link")]
        public string? Link { get; set; }

        [Required(ErrorMessage = "Не указан id типа материала!")]
        [Display(Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Duration")]
        public int? Duration { get; set; }

        [Display(Name = "Note")]
        public string? Note { get; set; }
    }
}
