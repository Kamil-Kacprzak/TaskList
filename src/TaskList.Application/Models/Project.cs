namespace TaskList.Application.Models
{
    public class Project
    {
        public string ProjectName { get; set; } = "";
        public long SequentialId { get; set; }
        public List<Task> Tasks { get; set; } = new ();
    }
}
