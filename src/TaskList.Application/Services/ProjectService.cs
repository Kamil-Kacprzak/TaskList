using System.Threading.Tasks;
using TaskList.Application.Interfaces;
using TaskList.Application.Models;

namespace TaskList.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        private readonly List<Project> _projects;
        private int _nextProjectId = 1;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _projects = _projectRepository.GetAllProjects();
        }
        public void AddProject(string projectName)
        {
            var project = new Project
            {
                ProjectName = projectName,
                SequentialId = _nextProjectId++
            };
            _projects.Add(project);
        }

        public void AddTaskToProject(Project project, Models.Task task)
        {
            if (project != null && task != null)
            {
                task.ProjectId = project.SequentialId;
                project.Tasks.Add(task);
            }
        }

        public Project? GetProjectByName(string projectName)
        {
            return _projects.FirstOrDefault(p => p.ProjectName == projectName);
        }

        public List<Models.Task> GetTasksByProject(string projectName)
        {
            return GetProjectByName(projectName)?.Tasks ?? new List<Models.Task>();
        }

        public void ShowTasksGroupedByProject()
        {
            foreach (var project in _projects)
            {
                Console.WriteLine(project.ProjectName);
                foreach (var task in project.Tasks)
                {
                    Console.WriteLine("    [{0}] {1}: {2}", task.IsDone ? 'x' : ' ', task.SequentialId, task.TaskName);
                }
                Console.WriteLine();
            }
        }

        public void ShowTasksWithTodayDeadlineDate()
        {
            var dateTime = DateTime.Now;
            var todaysDate = DateOnly.FromDateTime(dateTime);
            var projectsWithTodaysDeadlines = new Dictionary<Project, List<Models.Task>>();

            foreach (var project in _projects)
            {
                var tasks = new List<Models.Task>();
                foreach (var task in project.Tasks)
                {
                    if (task.DeadlineDate == todaysDate)
                    {
                        tasks.Add(task);
                    }
                }

                if (tasks.Count > 0)
                {
                    projectsWithTodaysDeadlines.Add(project, tasks);
                }
            }
            
            Console.WriteLine($"Deadline is: {todaysDate}");
            
            foreach (var project in projectsWithTodaysDeadlines)
            {
                Console.WriteLine(project.Key.ProjectName);
                foreach (var task in project.Value)
                {
                    Console.WriteLine("    [{0}] {1}: {2}", task.IsDone ? 'x' : ' ', task.SequentialId, task.TaskName);
                }
                Console.WriteLine();
            }
        }
    }
}
