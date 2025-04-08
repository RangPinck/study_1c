using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.AccountDTOs
{
    public class UpdateUserRoleDTO
    {
        [Required(ErrorMessage = "Не указан Id пользователя!")]
        [Display(Name = "UserId")]
        [DefaultValue("")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Не указан Id роли!")]
        [Display(Name = "RoleId")]
        [DefaultValue("")]
        public Guid RoleId { get; set; }
    }
}
