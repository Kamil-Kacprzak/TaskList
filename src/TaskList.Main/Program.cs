using System.Diagnostics;

IList<Process> _startedProcesses = new List<Process>();

int choice;

do
{
    ShowWelcomeMessage();

    var isParsed = int.TryParse(Console.ReadLine(), out choice);

    if (!isParsed)
    {
        ShowErrorResult();
        choice = -1;
        continue;
    }

    MatchUserChoice(choice);

} while (choice != 0);

void MatchUserChoice(int choice)
{
    switch (choice)
    {
        case 0:
            Console.Clear();
            Console.WriteLine("Exiting... See you soon :)");

            KillAllProcesses();

            break;
        case 1:
            Console.WriteLine("Running Console App...");

            StartConsoleApp();

            Console.Clear();
            Console.WriteLine("Web Api is running...");
            break;
        case 2:
            Console.WriteLine("Running Web Api...");

            StartWebApi();

            Console.Clear();
            Console.WriteLine("Web Api is running. Please navigate to provided url in console to use it.");
            break;
        default:
            ShowErrorResult();
            break;
    }
}

void StartConsoleApp()
{
    var webApiProjectPath = @"..\..\..\..\TaskList.ConsoleApp";
    var currentDirectory = Directory.GetCurrentDirectory();
    var fullPath = Path.Combine(currentDirectory, webApiProjectPath);

    StartProcess(fullPath);
}

void StartWebApi()
{
    var webApiProjectPath = @"..\..\..\..\TaskList.WebApi";
    var currentDirectory = Directory.GetCurrentDirectory();
    var fullPath = Path.Combine(currentDirectory, webApiProjectPath);

    StartProcess(fullPath);
}

void StartProcess(string fullPath)
{
    try
    {
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "run",
            WorkingDirectory = fullPath,
            UseShellExecute = true
        });

        if (process != null)
        {
            _startedProcesses.Add(process);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error starting Web API: {ex.Message}");
    }
}

void KillAllProcesses()
{
    foreach (var process in _startedProcesses)
    {
        if (!process.HasExited)
        {
            process.Kill();
        }
    }
}

void ShowWelcomeMessage()
{
    Console.WriteLine("Welcome to TaskList! Your task manager for multiple projects.\n");
    Console.WriteLine("Would you prefer to run it through:");
    Console.WriteLine("1. Console App");
    Console.WriteLine("2. Web Api");
    Console.WriteLine("0. Or just exit? :(");
    Console.WriteLine("\n");
    Console.Write("Enter the # of your choice: ");
}

void ShowErrorResult()
{
    Console.Clear();
    Console.WriteLine("--> Invalid choice. Please try again.");
    Console.WriteLine();
}