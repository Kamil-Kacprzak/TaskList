namespace TaskList.Application.Models
{
    public class Task
    {
        public string TaskName { get; set; } = "";

        public long SequentialId { get; set; }

        public bool IsDone { get; set; }

        public DateOnly Date { get; set; }
    }
}
