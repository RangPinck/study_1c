using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string UserLogin { get; set; } = null!;

    public string UserHashPassword { get; set; } = null!;

    public string UserSurname { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? UserPatronymic { get; set; }

    public DateTime UserDataCreate { get; set; }

    public int UserRole { get; set; }

    public bool IsFirst { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Role UserRoleNavigation { get; set; } = null!;

    public virtual ICollection<UsersTask> UsersTasks { get; set; } = new List<UsersTask>();
}
