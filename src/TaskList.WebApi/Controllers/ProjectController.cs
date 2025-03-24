using Microsoft.AspNetCore.Mvc;
using TaskList.Application.Interfaces;
using TaskListModels = TaskList.Application.Models;

namespace TaskList.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("", Name = "AddProject")]
        public async Task<IActionResult> AddProject(string projectName)
        {
            if (projectName is null || projectName.Length == 0)
            {
                return BadRequest();
            }

            try
            {
                await Task.Run(() => _projectService.AddProject(projectName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return CreatedAtAction("AddProject", new { projectName });
        }

        [HttpPost("/{project_id}/task", Name = "AddTaskToTheProject")]
        public async Task<IActionResult> AddTaskToTheProject(int project_id, string taskName)
        {
            if (taskName is null || taskName.Length == 0)
            {
                return BadRequest();
            }

            try
            {
                await Task.Run(() =>
                {
                    var project = _projectService.GetProjectById(project_id);
                    project?.Tasks.Add(new TaskListModels.Task { TaskName = taskName });
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return CreatedAtAction("AddTaskToTheProject", new { taskName });
        }

        //[HttpGet("tasks", Name = "GetTasks")]
        //public IEnumerable<TaskListModels.Task> GetTasks()
        //{
        //    var task = new TaskListModels.Task() 
        //    {
        //        TaskName = "task1",
        //        SequentialId = 1,
        //        DeadlineDate = new DateOnly(2022, 1, 1)
        //    };
        //    var tasks = new List<TaskListModels.Task>() { task };
        //    return tasks;
        //}
    }
}
