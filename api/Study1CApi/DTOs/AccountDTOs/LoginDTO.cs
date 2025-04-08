using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Study1CApi.DTOs.AuthDTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Не указана почта")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [DefaultValue("12345678")]
        public string Password { get; set; } = string.Empty;
    }
}
