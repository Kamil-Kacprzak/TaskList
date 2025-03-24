using TaskList.Application.Interfaces;
using TaskList.Application.Models;

namespace TaskList.Application.Persistance
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<Models.Task> _tasks = new();

        public List<Models.Task> GetAllTasks() => _tasks;
    }
}
