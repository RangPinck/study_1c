using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study1CApi.DTOs.MaterialDTOs
{
    public class MaterialDTO
    {
        public Guid MaterialId { get; set; }

        public string MaterialName { get; set; } = null!;

        public DateTime MaterialDateCreate { get; set; }

        public string? Link { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; } = null!;

        public string? Description { get; set; }

        public int Status { get; set; }

        public int? DurationMaterial { get; set; } = 0;

        public DateTime? DateStart { get; set; }
    }
}
