using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.BlockDTOs
{
    public class AddBlockDTO
    {
        [Required(ErrorMessage = "Не указано название блока!")]
        [Display(Name = "Block name")]
        [DefaultValue("")]
        public string BlockName { get; set; } = null!;

        [Required(ErrorMessage = "Не указан Id курса!")]
        [Display(Name = "Course Id")]
        [DefaultValue("")]
        public Guid Course { get; set; }

        [Display(Name = "Description")]
        [DefaultValue("")]
        public string? Description { get; set; }
    }
}
