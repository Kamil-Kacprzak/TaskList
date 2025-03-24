using TaskList.Application.Interfaces;
using TaskList.Application.Models;

namespace TaskList.Application.Persistance
{
    public class InMemoryProjectRepository : IProjectRepository
    {
        private readonly List<Project> _projects = new();

        public List<Project> GetAllProjects() => _projects;

    }
}
