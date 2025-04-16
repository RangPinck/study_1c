namespace Study1CApi.DTOs.BlockDTOs
{
    public class BlockStatisticsDTO
    {
        public Guid BlockId { get; set; }

        public int FullyCountTask { get; set; }

        public int FullyDurationNeeded { get; set; }

        public int CompletedTaskCount { get; set; }

        public int DurationCompletedTask { get; set; }

        public double PercentCompletedTask { get; set; }

        public double PercentDurationCompletedTask { get; set; }
    }
}
