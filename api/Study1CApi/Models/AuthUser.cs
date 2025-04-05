using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
