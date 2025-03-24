using TaskList.Application.Models;

namespace TaskList.Application.Interfaces
{
    public interface ITaskRepository
    {
        List<Models.Task> GetAllTasks();
    }
}
