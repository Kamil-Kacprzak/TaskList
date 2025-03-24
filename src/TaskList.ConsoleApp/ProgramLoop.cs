using TaskList.Application.Interfaces;
using TaskList.Application.Persistance;
using TaskList.Application.Services;

namespace TaskList.ConsoleApp
{
	public sealed class ProgramLoop
	{
		private const string QUIT = "quit";
		private const string startupText = "Welcome to TaskList! Type 'help' for available commands.";

		private readonly ITaskService _taskService;
		private readonly IProjectService _projectService;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        public ProgramLoop()
		{
            this._projectRepository = new InMemoryProjectRepository();
            this._taskRepository = new InMemoryTaskRepository();
            this._taskService = new TaskService(_taskRepository);
            this._projectService = new ProjectService(_projectRepository);
        }

        public void Run()
		{
			Console.WriteLine(startupText);
			while (true) {
				Console.Write("> ");

				var command = ParseCommand(Console.ReadLine());
				
				if(command.Length == 0)
                {
                    continue;
                }

                if (command[0] == QUIT) {
					break;
				}

				Execute(command);
			}
		}

        private string[] ParseCommand(string? input)
        {
			if (input == null)
			{
				return new string[] { };
			}

            string[] result = input
				.ToLowerInvariant()
				.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

			return result;
        }

        private void Execute(string[] command)
		{
            long id;
			switch (command[0]) {
				case "show":
					_projectService.ShowTasksGroupedByProject();
					break;
                case "today":
                    _projectService.ShowTasksWithTodayDeadlineDate();
                    break;
                case "add":
					EvaluateAddCommand(command);
					break;
                case "deadline":
                    AddDeadlineToTask(command);
                    break;
                case "check":
                    id = ValidateCheckingCommand(command);
                    _taskService.CheckTask(id);
                    break;
                case "uncheck":
					id = ValidateCheckingCommand(command);
                    _taskService.UncheckTask(id);
                    break;
                case "help":
					Help();
					break;
                case "clear":
                    Console.Clear();
                    Console.WriteLine(startupText);
                    break;
                default:
					Error(command[0]);
					break;
			}
		}

        private void AddDeadlineToTask(string[] command)
        {
            if(command.Length != 3)
            {
                Console.WriteLine("Invalid number of arguments");
                return;
            }

            if (!long.TryParse(command[1], out long id))
            {
                Console.WriteLine("Invalid task id");
                return;
            }

            if (!DateOnly.TryParse(command[2], out DateOnly date))
            {
                Console.WriteLine("Invalid date");
                return;
            }
            
            _taskService.UpdateTaskDeadline(id, date);
        }

        private long ValidateCheckingCommand(string[] command)
        {
            try
            {
                if (command.Length == 2)
                {
                    long.TryParse(command[1], out long id);
					return id;
                }
                else
                {
                    throw new ArgumentException("Wrong amount of parameters");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
			return -1;
        }

        private void EvaluateAddCommand(string[] command)
        {
            string subcommand = command[1];
            if (subcommand == "project" && command.Length == 3)
            {
				_projectService.AddProject(command[2]);
            }
            else if (subcommand == "task" && command.Length == 4)
            {
				var project = _projectService.GetProjectByName(command[2]);
                if (project == null)
				{
                    Console.WriteLine("Project not found");
                    return;
                }
				var task = _taskService.CreateTask(command[3]);
                _projectService.AddTaskToProject(project, task);
            }
            else
            {
                Console.WriteLine("Invalid arguments");
            }
        }	

		private void Help()
		{
			Console.WriteLine("Commands:");
			Console.WriteLine("  show");
			Console.WriteLine("  today");
			Console.WriteLine("  add project <project name>");
			Console.WriteLine("  add task <project name> <task description>");
			Console.WriteLine("  deadline <task ID> <date>");
			Console.WriteLine("  check <task ID>");
			Console.WriteLine("  uncheck <task ID>");
			Console.WriteLine("  clear");
            Console.WriteLine("  help");
            Console.WriteLine("  quit");
			Console.WriteLine();
		}

		private void Error(string command)
		{
			Console.WriteLine($"I don't know what the command \"{command}\" is.");
		}
	}
}
