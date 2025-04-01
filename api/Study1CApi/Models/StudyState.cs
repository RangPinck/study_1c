using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class StudyState
{
    public int StateId { get; set; }

    public string StateName { get; set; } = null!;

    public virtual ICollection<UsersTask> UsersTasks { get; set; } = new List<UsersTask>();
}
