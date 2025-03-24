using TaskList.Application.Interfaces;

namespace TaskList.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        private readonly List<Models.Task> _tasks;
        private int _nextTaskId = 1;

        public TaskService(ITaskRepository taskRepository)
        {
            this._taskRepository = taskRepository;
            _tasks = _taskRepository.GetAllTasks();
        }
        public Models.Task CreateTask(string description)
        {
            var task = new Models.Task
            {
                TaskName = description,
                SequentialId = _nextTaskId++,
                IsDone = false
            };
            _tasks.Add(task);
            return task;
        }

        public void CheckTask(long id)
        {
            var task = GetTask(id);

            if (task != null)
            {
                task.IsDone = true;
            }
            else
            {
                Console.WriteLine($"Could not find a task with an ID of {id}.");
            }
        }
        public void UncheckTask(long id)
        {
            var task = GetTask(id);

            if (task != null)
            {
                task.IsDone = false;
            }
            else
            {
                Console.WriteLine($"Could not find a task with an ID of {id}.");
            }
        }

        public void ShowTasks()
        {
            foreach (var task in _tasks)
            {
                Console.WriteLine("    [{0}] {1}: {2}", task.IsDone ? 'x' : ' ', task.SequentialId, task.TaskName);
            }
        }

        public Models.Task? GetTask(long id)
        {
            var task = _tasks.Find(task => task.SequentialId == id);
            return task ?? null;
        }

        public void UpdateTaskDeadline(long id, DateOnly deadline)
        {
            var task = GetTask(id);
            if (task == null)
            {
                Console.WriteLine("Task not found");
                return;
            }
            task.DeadlineDate = deadline;
            _taskRepository.UpdateTask(task);
        }
    }
}
