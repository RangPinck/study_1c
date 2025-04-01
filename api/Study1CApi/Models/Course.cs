using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class Course
{
    public Guid CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public DateTime CourseDataCreate { get; set; }

    public string? Description { get; set; }

    public string? Link { get; set; }

    public Guid Author { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;

    public virtual ICollection<CoursesBlock> CoursesBlocks { get; set; } = new List<CoursesBlock>();
}
