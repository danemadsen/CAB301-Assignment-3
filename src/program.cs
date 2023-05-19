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
                    LoadTasksMenu();
                    break;
                case "2":
                    SaveTasksMenu();
                    break;
                case "3":
                    AddTaskMenu();
                    break;
                case "4":
                    RemoveTaskMenu();
                    break;
                case "5":
                    ChangeDurationMenu();
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
    
    static private void LoadTasksMenu() {
        Console.Write("Enter the name of the file: ");
        string fileName = Console.ReadLine();
        
        if (string.IsNullOrEmpty(fileName)) 
        {
            Console.WriteLine("Invalid file name.");
            return;
        }

        try
        {
            taskManager.LoadTasks(fileName);
        }
        catch (Exception error)
        {
            Console.WriteLine("Error loading tasks from file: " + error.Message);
        }
    }

    static private void SaveTasksMenu() {
        Console.Write("Enter the name of the file: ");
        string fileName = Console.ReadLine();
        
        if (string.IsNullOrEmpty(fileName)) 
        {
            Console.WriteLine("Invalid file name.");
            return;
        }

        try
        {
            taskManager.SaveTasks(fileName);
            Console.WriteLine("Tasks saved successfully!");
        }
        catch (Exception error)
        {
            Console.WriteLine("Error saving tasks to file: " + error.Message);
        }
    }

    static private void AddTaskMenu() {
        try
        {
            Console.Write("Enter the task ID: ");
            string taskId = Console.ReadLine();
            
            if (string.IsNullOrEmpty(taskId) || taskId == " ") throw new Exception("Invalid task ID.");
            
            Console.Write("Enter the task Duration: ");
            int Duration = int.Parse(Console.ReadLine());

            Console.Write("Enter Dependencies or leave empty to skip: ");
            List<string> dependencies = Console.ReadLine().Split(',').Select(s => s.Trim()).ToList();

            taskManager.AddTask(taskId, Duration, dependencies);
            Console.WriteLine("Task added successfully!");
        }
        catch (Exception error)
        {
            Console.WriteLine("Error adding task: " + error.Message);
        }
    }

    static private void RemoveTaskMenu() {
        Console.Write("Enter the task ID to remove: ");
        string taskIdToRemove = Console.ReadLine();
        taskManager.RemoveTask(taskIdToRemove);
    }

    static private void ChangeDurationMenu() {
        Console.Write("Enter the task ID to change Duration: ");
        string taskIdToChange = Console.ReadLine();
        Console.Write("Enter the new Duration: ");
        int newDuration = int.Parse(Console.ReadLine());
        taskManager.SetDuration(taskIdToChange, newDuration);
    }
}
