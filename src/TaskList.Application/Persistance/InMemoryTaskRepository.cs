using TaskList.Application.Interfaces;
using TaskList.Application.Models;

namespace TaskList.Application.Persistance
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<Models.Task> _tasks = new();

        public List<Models.Task> GetAllTasks() => _tasks;

        public bool UpdateTask(Models.Task updatedTask)
        {
            var task = _tasks.FirstOrDefault(t => t.SequentialId == updatedTask.SequentialId);
            
            if(task == null)
            {
                return false;
            }
           
            task.TaskName = updatedTask.TaskName;
            task.IsDone = updatedTask.IsDone;
            task.DeadlineDate = updatedTask.DeadlineDate;
            task.ProjectId = updatedTask.ProjectId;
            
            return true;
        }
    }
}
