namespace Study1CApi.DTOs.TaskDTOs
{
    public class TaskDTO
    {
        public Guid TaskId { get; set; }

        public string TaskName { get; set; } = null!;

        public DateTime TaskDateCreated { get; set; }

        public int Duration { get; set; }

        public string? Link { get; set; }

        public int TaskNumberOfBlock { get; set; }

        public int StudyStateId { get; set; }

        public string? Description { get; set; }
    }
}