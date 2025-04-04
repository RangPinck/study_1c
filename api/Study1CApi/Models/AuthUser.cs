using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study1CApi.Models
{
    public partial class AuthUser
    {
        public Guid UserId { get; set; }
        public string UserLogin { get; set; } = null!;
        public string UserHashPassword { get; set; } = null!;
        public DateTime UserDataCreate { get; set; }
        public int UserRole { get; set; }
        public virtual Role UserRoleNavigation { get; set; } = null!;
    }
}
