namespace Study1CApi.Models;

public partial class Material
{
    public Guid MaterialId { get; set; }

    public string MaterialName { get; set; } = null!;

    public DateTime MaterialDateCreate { get; set; }

    public string? Link { get; set; }

    public int Type { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<BlocksMaterial> BlocksMaterials { get; set; } = new List<BlocksMaterial>();

    public virtual MaterialType TypeNavigation { get; set; } = null!;
}
