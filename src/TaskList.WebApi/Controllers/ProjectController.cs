using Microsoft.AspNetCore.Mvc;
using TaskListModels = TaskList.Application.Models;

namespace TaskList.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        [HttpGet("tasks", Name = "GetTasks")]
        public IEnumerable<TaskListModels.Task> GetTasks()
        {
            var task = new TaskListModels.Task() 
            {
                TaskName = "task1",
                SequentialId = 1,
                DeadlineDate = new DateOnly(2022, 1, 1)
            };
            var tasks = new List<TaskListModels.Task>() { task };
            return tasks;
        }
    }
}
