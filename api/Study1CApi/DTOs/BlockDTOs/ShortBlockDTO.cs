using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study1CApi.DTOs.BlockDTOs
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
