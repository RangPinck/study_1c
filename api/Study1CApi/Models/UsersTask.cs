using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class UsersTask
{
    public Guid UtId { get; set; }

    public Guid AuthUser { get; set; }

    public Guid? Task { get; set; }

    public Guid? Practice { get; set; }

    public Guid? Material { get; set; }

    public int Status { get; set; }

    public DateTime DateStart { get; set; }

    public int DurationTask { get; set; }

    public int DurationPractice { get; set; }

    public int DurationMaterial { get; set; }

    public virtual User AuthUserNavigation { get; set; } = null!;

    public virtual BlocksMaterial? MaterialNavigation { get; set; }

    public virtual TasksPractice? PracticeNavigation { get; set; }

    public virtual StudyState StatusNavigation { get; set; } = null!;

    public virtual BlocksTask? TaskNavigation { get; set; }
}
