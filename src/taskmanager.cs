using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class TaskManager
{
    private Dictionary<string, Task> tasks;

    public TaskManager()
    {
        tasks = new Dictionary<string, Task>();
    }

    public void LoadTasksFromFile(string fileName)
    {
        try
        {
            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                string taskId = parts[0].Trim();
                int timeNeeded = int.Parse(parts[1].Trim());

                if (!tasks.ContainsKey(taskId))
                {
                    tasks.Add(taskId, new Task(taskId, timeNeeded));
                }

                if (parts.Length > 2)
                {
                    for (int i = 2; i < parts.Length; i++)
                    {
                        string dependency = parts[i].Trim();
                        if (!tasks.ContainsKey(dependency))
                        {
                            tasks.Add(dependency, new Task(dependency));
                        }

                        tasks[taskId].AddDependency(tasks[dependency]);
                    }
                }
            }

            Console.WriteLine("Tasks loaded successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading tasks from file: " + ex.Message);
        }
    }

    public void AddTask(string taskId, int timeNeeded, List<string> dependencies)
    {
        if (!tasks.ContainsKey(taskId))
        {
            tasks.Add(taskId, new Task(taskId, timeNeeded));

            foreach (string dependency in dependencies)
            {
                if (!tasks.ContainsKey(dependency))
                {
                    tasks.Add(dependency, new Task(dependency));
                }

                tasks[taskId].AddDependency(tasks[dependency]);
            }

            Console.WriteLine("Task added successfully!");
        }
        else
        {
            Console.WriteLine("Task with the same ID already exists!");
        }
    }

    public void RemoveTask(string taskId)
    {
        if (tasks.ContainsKey(taskId))
        {
            tasks.Remove(taskId);
            foreach (Task task in tasks.Values)
            {
                task.RemoveDependency(taskId);
            }

            Console.WriteLine("Task removed successfully!");
        }
        else
        {
            Console.WriteLine("Task not found!");
        }
    }

    public void ChangeTimeNeeded(string taskId, int timeNeeded)
    {
        if (tasks.ContainsKey(taskId))
        {
            tasks[taskId].TimeNeeded = timeNeeded;
            Console.WriteLine("Time needed changed successfully!");
        }
        else
        {
            Console.WriteLine("Task not found!");
        }
    }

    public void SaveTasksToFile(string fileName)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (Task task in tasks.Values)
                {
                    writer.Write(task.Id + ", " + task.TimeNeeded);
                    foreach (string dependency in task.Dependencies)
                    {
                        writer.Write(", " + dependency);
                    }
                    writer.WriteLine();
                }
            }

            Console.WriteLine("Tasks saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving tasks to file: " + ex.Message);
        }
    }

    public void FindTaskSequence()
    {
        List<Task> sortedTasks = new List<Task>();
        HashSet<Task> visited = new HashSet<Task>();

        foreach (Task task in tasks.Values)
        {
            Visit(task, visited, sortedTasks);
        }

        string sequence = string.Join(", ", sortedTasks.Select(t => t.Id));
        try
        {
            File.WriteAllText("Sequence.txt", sequence);
            Console.WriteLine("Task sequence saved to Sequence.txt!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving task sequence: " + ex.Message);
        }
    }

    private void Visit(Task task, HashSet<Task> visited, List<Task> sortedTasks)
    {
        if (!visited.Contains(task))
        {
            visited.Add(task);

            foreach (Task dependency in task.Dependencies)
            {
                Visit(dependency, visited, sortedTasks);
            }

            sortedTasks.Add(task);
        }
    }

    public void FindEarliestTimes()
    {
        Dictionary<Task, int> earliestTimes = new Dictionary<Task, int>();

        foreach (Task task in tasks.Values)
        {
            CalculateEarliestTime(task, earliestTimes);
        }

        try
        {
            using (StreamWriter writer = new StreamWriter("EarliestTimes.txt"))
            {
                foreach (var kvp in earliestTimes)
                {
                    writer.WriteLine(kvp.Key.Id + ", " + kvp.Value);
                }
            }

            Console.WriteLine("Earliest times saved to EarliestTimes.txt!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving earliest times: " + ex.Message);
        }
    }

    private int CalculateEarliestTime(Task task, Dictionary<Task, int> earliestTimes)
    {
        if (earliestTimes.ContainsKey(task))
        {
            return earliestTimes[task];
        }

        int maxDependencyTime = 0;
        foreach (Task dependency in task.Dependencies)
        {
            int dependencyTime = CalculateEarliestTime(dependency, earliestTimes);
            maxDependencyTime = Math.Max(maxDependencyTime, dependencyTime);
        }

        int earliestTime = maxDependencyTime + task.TimeNeeded;
        earliestTimes[task] = earliestTime;
        return earliestTime;
    }
}