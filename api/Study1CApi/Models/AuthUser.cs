using Microsoft.AspNetCore.Identity;

namespace Study1CApi.Models
{
    public partial class AuthUser : IdentityUser<Guid>
    {
        public User UserNavigation { get; set; }

        public AuthUser() { }

        public AuthUser(string userName) : this()
        {
            UserName = userName;
        }
    }
}
