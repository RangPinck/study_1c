using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class UserCourse
{
    public Guid CuId { get; set; }

    public Guid CourseId { get; set; }

    public Guid UserId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
