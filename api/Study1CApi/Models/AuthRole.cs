using Microsoft.AspNetCore.Identity;

namespace Study1CApi.Models
{
    public partial class AuthRole : IdentityRole<Guid>
    {
        public AuthRole() { }

        public AuthRole(string roleName) : this()
        {
            Name = roleName;
        }
    }
}
