using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.AccountDTOs
{
    public class UpdatePasswordDTO
    {
        [Required(ErrorMessage = "Не указана почта!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не указан пароль!")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Длина должна быть от 6 до 64 символов")]
        [DefaultValue("12345678")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не указан повторяющийся пароль!")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Длина должна быть от 6 до 64 символов")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DefaultValue("12345678")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
