using TaskListModels = TaskList.Application.Models;

namespace TaskList.ConsoleApp
{
	public sealed class ProgramLoop
	{
		private const string QUIT = "quit";
		private const string startupText = "Welcome to TaskList! Type 'help' for available commands.";

		private readonly IDictionary<string, IList<TaskListModels.Task>> tasks = new Dictionary<string, IList<TaskListModels.Task>>();

		private long lastId = 0;


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
			switch (command[0]) {
				case "show":
					Show();
					break;
				case "add":
					Add(command[1]);
					break;
				case "check":
					Check(command[1]);
					break;
				case "uncheck":
					Uncheck(command[1]);
					break;
				case "help":
					Help();
					break;
				default:
					Error(command[0]);
					break;
			}
		}

		private void Show()
		{
			foreach (var project in tasks) {
				Console.WriteLine(project.Key);
				foreach (var task in project.Value) {
					Console.WriteLine("    [{0}] {1}: {2}", task.IsDone ? 'x' : ' ', task.SequentialId, task.TaskName);
				}
				Console.WriteLine();
			}
		}

		private void Add(string commandLine)
		{
			var subcommandRest = commandLine.Split(" ".ToCharArray(), 2);
			var subcommand = subcommandRest[0];
			if (subcommand == "project") {
				AddProject(subcommandRest[1]);
			} else if (subcommand == "task") {
				var projectTask = subcommandRest[1].Split(" ".ToCharArray(), 2);
				if (projectTask.Length > 1) {
					AddTask(projectTask[0], projectTask[1]);
				} else {
                    Console.WriteLine("Invalid arguments"); 
				}
            }
		}

		private void AddProject(string name)
		{
			tasks[name] = new List<TaskListModels.Task>();
		}

		private void AddTask(string project, string description)
		{
			if (!tasks.TryGetValue(project, out IList<TaskListModels.Task> projectTasks))
			{
                Console.WriteLine("Could not find a project with the name \"{0}\".", project);
				return;
			}
			projectTasks.Add(new TaskListModels.Task { SequentialId = NextId(), TaskName = description, IsDone = false });
		}

		private void Check(string idString)
		{
			SetDone(idString, true);
		}

		private void Uncheck(string idString)
		{
			SetDone(idString, false);
		}

		private void SetDone(string idString, bool done)
		{
			int id = int.Parse(idString);
			var identifiedTask = tasks
				.Select(project => project.Value.FirstOrDefault(task => task.SequentialId == id))
				.Where(task => task != null)
				.FirstOrDefault();
			if (identifiedTask == null) {
				Console.WriteLine($"Could not find a task with an ID of {id}.");
				return;
			}

			identifiedTask.IsDone = done;
		}

		private void Help()
		{
			Console.WriteLine("Commands:");
			Console.WriteLine("  show");
			Console.WriteLine("  add project <project name>");
			Console.WriteLine("  add task <project name> <task description>");
			Console.WriteLine("  check <task ID>");
			Console.WriteLine("  uncheck <task ID>");
			Console.WriteLine();
		}

		private void Error(string command)
		{
			Console.WriteLine($"I don't know what the command \"{command}\" is.");
		}

		private long NextId()
		{
			return ++lastId;
		}
	}
}
