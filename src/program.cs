using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();
        string fileName = string.Empty;

        while (true)
        {
            Console.WriteLine("======== Project Management System ========");
            Console.WriteLine("1. Load tasks from a file");
            Console.WriteLine("2. Add a new task");
            Console.WriteLine("3. Remove a task");
            Console.WriteLine("4. Change time needed");
            Console.WriteLine("5. Save tasks to a file");
            Console.WriteLine("6. Find task sequence");
            Console.WriteLine("7. Find earliest times");
            Console.WriteLine("8. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter the name of the file: ");
                    fileName = Console.ReadLine();
                    taskManager.LoadTasks(fileName);
                    break;
                case "2":
                    Console.Write("Enter the task ID: ");
                    string taskId = Console.ReadLine();
                    Console.Write("Enter the time needed: ");
                    int Duration = int.Parse(Console.ReadLine());
                    Console.Write("Enter dependencies (comma-separated): ");
                    List<string> dependencies = Console.ReadLine().Split(',').Select(s => s.Trim()).ToList();
                    taskManager.AddTask(taskId, Duration, dependencies);
                    break;
                case "3":
                    Console.Write("Enter the task ID to remove: ");
                    string taskIdToRemove = Console.ReadLine();
                    taskManager.RemoveTask(taskIdToRemove);
                    break;
                case "4":
                    Console.Write("Enter the task ID to change time needed: ");
                    string taskIdToChange = Console.ReadLine();
                    Console.Write("Enter the new time needed: ");
                    int newDuration = int.Parse(Console.ReadLine());
                    taskManager.SetDuration(taskIdToChange, newDuration);
                    break;
                case "5":
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        taskManager.SaveTasks(fileName);
                    }
                    else
                    {
                        Console.WriteLine("No file loaded. Please load tasks from a file first.");
                    }
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
}
