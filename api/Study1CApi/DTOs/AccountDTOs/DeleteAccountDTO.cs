using System.ComponentModel.DataAnnotations;

namespace Study1CApi.DTOs.AuthDTO
{
    public class DeleteAccountDTO
    {
        [Required(ErrorMessage = "Id пользователя, соверщающего операцию не указан!")]
        [Display(Name = "UserId")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Id удаляемого пользователя не указан!")]
        [Display(Name = "UserIdWillBeDelete")]
        public Guid UserIdWillBeDelete { get; set; }
    }
}
