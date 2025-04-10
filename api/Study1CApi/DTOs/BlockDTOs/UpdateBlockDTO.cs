namespace Study1CApi.DTOs.BlockDTOs
{
    public class UpdateBlockDTO
    {
        public Guid BlockId { get; set; }

        public string BlockName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
