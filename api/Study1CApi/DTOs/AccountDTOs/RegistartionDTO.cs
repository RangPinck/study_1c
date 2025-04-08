using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Study1CApi.DTOs.AuthDTO
{
    public class RegistartionDTO
    {
        public bool IsFirst { get; set; }

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

        [Required(ErrorMessage = "Id роли пользователя не указан!")]
        [Display(Name = "Id роли пользователя")]
        [DefaultValue("f47ac10b-58cc-4372-a567-0e02b2c3d479")]
        public Guid RoleId { get; set; } = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479");
    }
}
