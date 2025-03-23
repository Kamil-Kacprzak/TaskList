using Microsoft.AspNetCore.Mvc;

namespace TaskList.WebApi.Controllers
{
    //TODO: KKA - replace this with the correct controller content
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Project> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Project
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("tasks", Name = "GetTasks")]
        public IEnumerable<string> GetTasks()
        {
            var tasks = Enumerable.Range(1, 5).Select(index => "task" + index).ToArray();
            return tasks;
        }
    }
}
