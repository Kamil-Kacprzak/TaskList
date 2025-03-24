using TaskList.Application.Persistance;
using TaskList.Application.Services;

namespace TaskList.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();

            new ProgramLoop().Run();
        }

    }
}
