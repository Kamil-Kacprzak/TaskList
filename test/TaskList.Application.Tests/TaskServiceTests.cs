using Moq;
using TaskList.Application.Interfaces;
using TaskList.Application.Models;
using TaskList.Application.Services;
using Xunit;

namespace TaskList.Application.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IProjectService> _projectServiceMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _projectServiceMock = new Mock<IProjectService>();
            _taskRepositoryMock.Setup(repo => repo.GetAllTasks()).Returns(new List<Models.Task>());
            _taskService = new TaskService(_taskRepositoryMock.Object, _projectServiceMock.Object);
        }

        [Fact]
        public void CreateTask_ShouldCreateTask()
        {
            // Arrange
            var description = "Test Task";

            // Act
            var task = _taskService.CreateTask(description);

            // Assert
            Assert.NotNull(task);
            Assert.Equal(description, task.TaskName);
            Assert.False(task.IsDone);
        }

        [Fact]
        public void CheckTask_ShouldMarkTaskAsDone()
        {
            // Arrange
            var task = new Models.Task { SequentialId = 1, TaskName = "Test Task", IsDone = false };
            _taskRepositoryMock.Setup(repo => repo.GetAllTasks()).Returns(new List<Models.Task> { task });
            var taskService = new TaskService(_taskRepositoryMock.Object, _projectServiceMock.Object);

            // Act
            taskService.CheckTask(1);

            // Assert
            Assert.True(task.IsDone);
        }

        [Fact]
        public void UncheckTask_ShouldMarkTaskAsNotDone()
        {
            // Arrange
            var task = new Models.Task { SequentialId = 1, TaskName = "Test Task", IsDone = true };
            _taskRepositoryMock.Setup(repo => repo.GetAllTasks()).Returns(new List<Models.Task> { task });
            var taskService = new TaskService(_taskRepositoryMock.Object, _projectServiceMock.Object);

            // Act
            taskService.UncheckTask(1);

            // Assert
            Assert.False(task.IsDone);
        }

        [Fact]
        public void GetTask_ShouldReturnTask()
        {
            // Arrange
            var task = new Models.Task { SequentialId = 1, TaskName = "Test Task" };
            _taskRepositoryMock.Setup(repo => repo.GetAllTasks()).Returns(new List<Models.Task> { task });
            var taskService = new TaskService(_taskRepositoryMock.Object, _projectServiceMock.Object);

            // Act
            var result = taskService.GetTask(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.TaskName, result.TaskName);
        }

        [Fact]
        public void UpdateTaskDeadline_ShouldUpdateDeadline()
        {
            // Arrange
            var task = new Models.Task { SequentialId = 1, TaskName = "Test Task" };
            var deadline = new DateOnly(2025, 12, 31);
            _taskRepositoryMock.Setup(repo => repo.GetAllTasks()).Returns(new List<Models.Task> { task });
            var taskService = new TaskService(_taskRepositoryMock.Object, _projectServiceMock.Object);

            // Act
            taskService.UpdateTaskDeadline(1, deadline);

            // Assert
            Assert.Equal(deadline, task.DeadlineDate);
            _taskRepositoryMock.Verify(repo => repo.UpdateTask(task), Times.Once);
        }

        [Fact]
        public void ViewTasksByDeadline_ShouldGroupTasksByDeadline()
        {
            // Arrange
            var task1 = new Models.Task { SequentialId = 1, TaskName = "Task 1", DeadlineDate = new DateOnly(2025, 12, 31), ProjectId = 1 };
            var task2 = new Models.Task { SequentialId = 2, TaskName = "Task 2", DeadlineDate = new DateOnly(2025, 12, 31), ProjectId = 1 };
            var task3 = new Models.Task { SequentialId = 3, TaskName = "Task 3", DeadlineDate = null, ProjectId = 2 };
            _taskRepositoryMock.Setup(repo => repo.GetAllTasks()).Returns(new List<Models.Task> { task1, task2, task3 });
            _projectServiceMock.Setup(service => service.GetProjectById(1)).Returns(new Project { ProjectName = "Project 1" });
            _projectServiceMock.Setup(service => service.GetProjectById(2)).Returns(new Project { ProjectName = "Project 2" });
            var taskService = new TaskService(_taskRepositoryMock.Object, _projectServiceMock.Object);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                taskService.ViewTasksByDeadline();

                // Assert
                var expectedOutput = "Deadline: 2025-12-31\n  Project: 1\n    - Task: Task 1\n    - Task: Task 2\nNo deadline\n  Project: Project 1\n    - Task: Task 1\n    - Task: Task 2\n";
                Assert.Equal(expectedOutput, sw.ToString().Replace("\r\n", "\n"));
            }
        }
    }
}
