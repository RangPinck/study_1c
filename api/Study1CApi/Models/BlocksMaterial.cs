using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class BlocksMaterial
{
    public Guid BmId { get; set; }

    public Guid Block { get; set; }

    public Guid Material { get; set; }

    public DateTime BmDateCreate { get; set; }

    public string? Note { get; set; }

    public int? Duration { get; set; }

    public virtual CoursesBlock BlockNavigation { get; set; } = null!;

    public virtual Material MaterialNavigation { get; set; } = null!;

    public virtual ICollection<UsersTask> UsersTasks { get; set; } = new List<UsersTask>();
}
