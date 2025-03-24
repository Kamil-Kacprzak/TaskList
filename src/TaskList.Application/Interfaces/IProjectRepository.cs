using TaskList.Application.Models;

namespace TaskList.Application.Interfaces
{
    public interface IProjectRepository
    {
        List<Project> GetAllProjects();
    }
}
