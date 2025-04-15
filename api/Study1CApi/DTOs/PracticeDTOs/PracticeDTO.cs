namespace Study1CApi.DTOs.PracticeDTOs
{
    public class PracticeDTO
    {
        public Guid PracticeId { get; set; }

        public string PracticeName { get; set; } = null!;

        public DateTime PracticeDateCreated { get; set; }

        public int DurationNeeded { get; set; }

        public string? Link { get; set; }

        public Guid Task { get; set; }

        public int? NumberPracticeOfTask { get; set; }

        public DateTime? DateStart { get; set; }

        public int Duration { get; set; }

        public int Status { get; set; }
    }
}
