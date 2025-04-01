using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class CoursesBlock
{
    public Guid BlockId { get; set; }

    public string BlockName { get; set; } = null!;

    public DateTime BlockDateCreated { get; set; }

    public Guid Course { get; set; }

    public string? Description { get; set; }

    public int? BlockNumberOfCourse { get; set; }

    public virtual ICollection<BlocksMaterial> BlocksMaterials { get; set; } = new List<BlocksMaterial>();

    public virtual ICollection<BlocksTask> BlocksTasks { get; set; } = new List<BlocksTask>();

    public virtual Course CourseNavigation { get; set; } = null!;
}
