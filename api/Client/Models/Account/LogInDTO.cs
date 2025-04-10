using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Account
{
    public class LogInDTO
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
