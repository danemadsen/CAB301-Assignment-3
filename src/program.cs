class Program
{
    static private TaskManager taskManager = new TaskManager();
    static private string fileName = null;

    static void Main(string[] args)
    {
        while (true)
        {
            switch (MainMenu())
            {
                case "1":
                    taskManager.LoadTasks();
                    break;
                case "2":
                    taskManager.SaveTasks();
                    break;
                case "3":
                    taskManager.AddTask();
                    break;
                case "4":
                    taskManager.RemoveTask();
                    break;
                case "5":
                    taskManager.SetNewDuration();
                    break;
                case "6":
                    taskManager.GetTaskSequence();
                    break;
                case "7":
                    taskManager.GetEarliestTimes();
                    break;
                case "8":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static private String MainMenu() {
        Console.WriteLine("<<<<<<<<< Project Management System >>>>>>>>>");
        Console.WriteLine("1. Load tasks from a file");
        Console.WriteLine("2. Save tasks to a file");
        Console.WriteLine("3. Add a new task");
        Console.WriteLine("4. Remove a task");
        Console.WriteLine("5. Change Duration");
        Console.WriteLine("6. Find task sequence");
        Console.WriteLine("7. Find earliest times");
        Console.WriteLine("8. Exit");
        Console.Write("Enter your choice: ");
        return Console.ReadLine();
    }
}
