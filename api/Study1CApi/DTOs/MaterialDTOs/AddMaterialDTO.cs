using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study1CApi.DTOs.MaterialDTOs
{
    public class AddMaterialDTO
    {
        public string MaterialName { get; set; } = null!;

        public string? Link { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
