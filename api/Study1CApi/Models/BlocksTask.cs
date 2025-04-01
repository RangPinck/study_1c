using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class BlocksTask
{
    public Guid TaskId { get; set; }

    public string TaskName { get; set; } = null!;

    public DateTime TaskDateCreated { get; set; }

    public int Duration { get; set; }

    public Guid Block { get; set; }

    public string? Link { get; set; }

    public int TaskNumberOfBlock { get; set; }

    public string? Description { get; set; }

    public virtual CoursesBlock BlockNavigation { get; set; } = null!;

    public virtual ICollection<TasksPractice> TasksPractices { get; set; } = new List<TasksPractice>();

    public virtual ICollection<UsersTask> UsersTasks { get; set; } = new List<UsersTask>();
}
