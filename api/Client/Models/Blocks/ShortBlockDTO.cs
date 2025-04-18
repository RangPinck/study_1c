﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Blocks
{
    public class ShortBlockDTO
    {
        public Guid BlockId { get; set; }

        public string BlockName { get; set; } = null!;

        public DateTime BlockDateCreated { get; set; }

        public string? Description { get; set; }

        public int? BlockNumberOfCourse { get; set; }
    }
}
