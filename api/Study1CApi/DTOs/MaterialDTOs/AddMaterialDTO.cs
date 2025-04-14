namespace Study1CApi.DTOs.MaterialDTOs
{
    public class AddMaterialDTO
    {
        public string MaterialName { get; set; } = null!;

        public string? Link { get; set; }

        public int TypeId { get; set; }

        public string? Description { get; set; }

        public int? Duration { get; set; }

        public Guid Block { get; set; }

        public string? Note { get; set; }
    }
}
