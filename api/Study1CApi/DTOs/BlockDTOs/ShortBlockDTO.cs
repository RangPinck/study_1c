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
