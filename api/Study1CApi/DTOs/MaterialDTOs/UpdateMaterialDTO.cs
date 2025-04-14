namespace Study1CApi.DTOs.MaterialDTOs
{
    public class UpdateMaterialDTO
    {
        public Guid MaterialId { get; set; }

        public string MaterialName { get; set; } = null!;

        public string? Link { get; set; }

        public int TypeId { get; set; }

        public string? Description { get; set; }

        public int? Duration { get; set; }

        public string? Note { get; set; }
    }
}
