namespace Study1CApi.DTOs.TaskDTOs
{
    public class TaskDTO
    {
        public Guid TaskId { get; set; }

        public string TaskName { get; set; } = null!;

        public DateTime TaskDateCreated { get; set; }

        public int DurationNeeded { get; set; }

        public string? Link { get; set; }

        public int TaskNumberOfBlock { get; set; }

        public string? Description { get; set; }

        public int Status { get; set; }

        public DateTime? DateStart { get; set; }

        public int Duration { get; set; }
    }
}