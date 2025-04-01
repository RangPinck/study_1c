using System;
using System.Collections.Generic;

namespace Study1CApi.Models;

public partial class MaterialType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
