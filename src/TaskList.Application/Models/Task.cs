namespace TaskList.Application.Models
{
    public class Task
    {
        public string TaskName { get; set; } = "";

        public long SequentialId { get; set; }

        public bool IsDone { get; set; }

        public DateOnly? DeadlineDate { get; set; } = null;
        public long ProjectId { get; set; }
    }
}
