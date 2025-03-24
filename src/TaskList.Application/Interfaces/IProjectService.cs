using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskList.Application.Models;

namespace TaskList.Application.Interfaces
{
    public interface IProjectService
    {
        void AddProject(string projectName);
        void AddTaskToProject(Project project, Models.Task task);
        List<Models.Task> GetTasksByProject(string projectName);
        Project? GetProjectByName(string projectName);
        Project? GetProjectById(long projectId);
        void ShowTasksGroupedByProject();
        void ShowTasksWithTodayDeadlineDate();
    }
}
