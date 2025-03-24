namespace TaskList.Application.Interfaces
{
    public interface ITaskService
    {
        Models.Task CreateTask(string description);
        void CheckTask(long id);
        void UncheckTask(long id);
        void ShowTasks();
        Models.Task? GetTask(long id);
        void UpdateTaskDeadline(long id, DateOnly deadline);
        void ViewTasksByDeadline();
    }
}
