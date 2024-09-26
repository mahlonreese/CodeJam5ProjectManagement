namespace CodeJam5ProjectManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Hello, User!");
            bool exit = false;
            while (!exit) {
                Console.WriteLine("\nWelcome to Student Data Manager, please make a selection: \n");
                Console.WriteLine("q    Quit the application.");
                Console.WriteLine("c    Add a story.");
                Console.WriteLine("r    View all stories");
                Console.WriteLine("u    Move a story.");
                Console.WriteLine("d    Delete a story");
                Console.WriteLine("v    View a specific story");
                Console.WriteLine("e    View employee stories");
                Console.WriteLine("a    Assign story to employee\n");

                var input = Console.ReadKey(intercept: true).Key;
                switch (input)
                {
                    case ConsoleKey.Q:
                        exit = true;
                        break;
                    case ConsoleKey.C:
                        DbMethod.AddStory();
                        break;
                    case ConsoleKey.R:
                        DbMethod.ViewAllStories();
                        break;
                    case ConsoleKey.U:
                        DbMethod.MoveStory();
                        break;
                    case ConsoleKey.D:
                        DbMethod.DeleteStory();
                        break;
                    case ConsoleKey.V:
                        DbMethod.ViewSpecificStory();
                        break;
                    case ConsoleKey.E:
                        DbMethod.ViewEmployeeStories();
                        break;
                    case ConsoleKey.A:
                        DbMethod.AssignEmployee();
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Try again.");
                        break;
                }
            }

            
        }


    }
}
