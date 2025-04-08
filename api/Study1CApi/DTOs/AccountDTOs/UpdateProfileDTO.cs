using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Study1CApi.DTOs.AccountDTOs
{
    public class UpdateProfileDTO
    {
        [Required(ErrorMessage = "Не указан Id пользователя!")]
        [Display(Name = "UserId")]
        [DefaultValue("")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Не указана фамилия!")]
        [Display(Name = "Surname")]
        [DefaultValue("")]
        public string UserSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не указано имя!")]
        [Display(Name = "Name")]
        [DefaultValue("")]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "Patronymic")]
        [DefaultValue("")]
        public string? UserPatronymic { get; set; }

        [Required(ErrorMessage = "Не указана почта!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
