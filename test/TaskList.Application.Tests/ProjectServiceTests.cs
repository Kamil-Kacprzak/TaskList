using global::TaskList.Application.Interfaces;
using global::TaskList.Application.Models;
using global::TaskList.Application.Services;
using ModelTask = global::TaskList.Application.Models;
using Moq;

namespace TaskList.Application.Tests
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _projectRepositoryMock.Setup(repo => repo.GetAllProjects()).Returns(new List<Project>());
            _projectService = new ProjectService(_projectRepositoryMock.Object);
        }

        [Fact]
        public void AddProject_ShouldAddProject()
        {
            // Arrange
            var projectName = "Test Project";

            // Act
            var project = _projectService.AddProject(projectName);

            // Assert
            Assert.NotNull(project);
            Assert.Equal(projectName, project.ProjectName);
        }

        [Fact]
        public void AddTaskToProject_ShouldAddTask()
        {
            // Arrange
            var project = new Project { SequentialId = 1, ProjectName = "Test Project", Tasks = new List<ModelTask.Task>() };
            var task = new ModelTask.Task { TaskName = "Test Task" };

            // Act
            _projectService.AddTaskToProject(project, task);

            // Assert
            Assert.Contains(task, project.Tasks);
            Assert.Equal(project.SequentialId, task.ProjectId);
        }

        [Fact]
        public void GetProjectById_ShouldReturnProject()
        {
            // Arrange
            var project = new Project { SequentialId = 1, ProjectName = "Test Project" };
            _projectRepositoryMock.Setup(repo => repo.GetAllProjects()).Returns(new List<Project> { project });
            var projectService = new ProjectService(_projectRepositoryMock.Object);

            // Act
            var result = projectService.GetProjectById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.ProjectName, result.ProjectName);
        }

        [Fact]
        public void GetProjectByName_ShouldReturnProject()
        {
            // Arrange
            var project = new Project { SequentialId = 1, ProjectName = "Test Project" };
            _projectRepositoryMock.Setup(repo => repo.GetAllProjects()).Returns(new List<Project> { project });
            var projectService = new ProjectService(_projectRepositoryMock.Object);

            // Act
            var result = projectService.GetProjectByName("Test Project");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.ProjectName, result.ProjectName);
        }
    }
}
