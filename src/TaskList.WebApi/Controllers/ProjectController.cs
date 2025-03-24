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

        [HttpPost("/project", Name = "AddProject")]
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
