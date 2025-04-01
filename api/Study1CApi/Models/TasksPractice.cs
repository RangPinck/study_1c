using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class TasksPractice
{
    public Guid PracticeId { get; set; }

    public string PracticeName { get; set; } = null!;

    public DateTime PracticeDateCreated { get; set; }

    public int Duration { get; set; }

    public string? Link { get; set; }

    public Guid Task { get; set; }

    public int? NumberPracticeOfTask { get; set; }

    public virtual BlocksTask TaskNavigation { get; set; } = null!;

    public virtual ICollection<UsersTask> UsersTasks { get; set; } = new List<UsersTask>();
}
