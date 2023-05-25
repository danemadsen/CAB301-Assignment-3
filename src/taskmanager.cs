class TaskManager
{
    private Dictionary<string, Task> tasks;

    public TaskManager()
    {
        tasks = new Dictionary<string, Task>();
    }

    public void LoadTasks()
    {
        Console.Write("Enter the name of the file: ");
        string fileName = Console.ReadLine();

        try
        {
            if (string.IsNullOrEmpty(fileName)) throw new Exception("Invalid file name.");
            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                string taskId = parts[0].Trim();
                int Duration = int.Parse(parts[1].Trim());

                if (!tasks.ContainsKey(taskId)) tasks.Add(taskId, new Task(taskId, Duration));
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
        }
        catch (Exception error)
        {
            Console.WriteLine("Error loading tasks from file: " + error.Message);
        }
    }

    public void SaveTasks()
    {
        Console.Write("Enter the name of the file: ");
        string fileName = Console.ReadLine();
        
        if (string.IsNullOrEmpty(fileName)) 
        {
            Console.WriteLine("Invalid file name.");
            return;
        }

        try
        {
            StreamWriter writer = new StreamWriter(fileName);
        
            foreach (Task task in tasks.Values)
            {
                writer.Write(task.Id + ", " + task.Duration);

                if (task.Dependencies.Count > 0)
                {
                    writer.Write(", ");
                    List<String> dependencyIds = new List<String>();

                    foreach (Task dependency in task.Dependencies)
                    {
                        dependencyIds.Add(dependency.Id);
                    }

                    writer.Write(string.Join(", ", dependencyIds));
                }

                writer.WriteLine();
            }

            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }

            Console.WriteLine("Tasks saved successfully!");
        }
        catch (Exception error)
        {
            Console.WriteLine("Error saving tasks to file: " + error.Message);
        }
    }

    public void AddTask()
    {
        try
        {
            Console.Write("Enter the task ID: ");
            string taskId = Console.ReadLine();
            
            if (string.IsNullOrEmpty(taskId) || taskId == " ") throw new Exception("Invalid task ID.");
            
            Console.Write("Enter the task Duration: ");
            int Duration = int.Parse(Console.ReadLine());

            Console.Write("Enter Dependencies or leave empty to skip: ");
            List<string> dependencyIds = Console.ReadLine().Split(',').Select(s => s.Trim()).ToList();

            if (tasks.ContainsKey(taskId)) throw new Exception("Task already exists!");
            tasks.Add(taskId, new Task(taskId, Duration));

            foreach (string Id in dependencyIds)
            {
                if (string.IsNullOrEmpty(Id) && string.IsNullOrWhiteSpace(Id)) break;
                if (tasks.ContainsKey(Id)) tasks[taskId].AddDependency(tasks[Id]);               
            }

            Console.WriteLine("Task added successfully!");
        }
        catch (Exception error)
        {
            Console.WriteLine("Error adding task: " + error.Message);
        }
    }

    public void RemoveTask()
    {
        try
        {
            Console.Write("Enter the task ID to remove: ");
            string taskId = Console.ReadLine();
            if (!tasks.ContainsKey(taskId)) throw new Exception("Task not found!");
            tasks.Remove(taskId);
            foreach (Task task in tasks.Values) task.RemoveDependency(taskId);
        }
        catch (Exception error)
        {
            Console.WriteLine("Error removing task: " + error.Message);
        }
    }

    public void SetNewDuration()
    {
        try
        {
            Console.Write("Enter the task ID to change Duration: ");
            string taskId = Console.ReadLine();

            if (string.IsNullOrEmpty(taskId) || taskId == " ") throw new Exception("Invalid task ID.");

            Console.Write("Enter the new Duration: ");
            int newDuration = int.Parse(Console.ReadLine());

            if (!tasks.ContainsKey(taskId)) throw new Exception("Task not found!");
            tasks[taskId].Duration = newDuration;
        }
        catch (Exception error)
        {
            Console.WriteLine("Error setting Duration: " + error.Message);
        }
    }

    public void GetTaskSequence()
    {
        List<Task> sortedTasks = new List<Task>();
        HashSet<Task> visited = new HashSet<Task>();

        foreach (Task task in tasks.Values)
        {
            VisitTask(task, visited, sortedTasks);
        }

        string sequence = string.Join(", ", sortedTasks.Select(t => t.Id));
        try
        {
            File.WriteAllText("Sequence.txt", sequence);
            Console.WriteLine("Task sequence saved to Sequence.txt!");
        }
        catch (Exception error)
        {
            Console.WriteLine("Error saving task sequence: " + error.Message);
        }
    }

    private void VisitTask(Task task, HashSet<Task> visited, List<Task> sortedTasks)
    {
        if (!visited.Contains(task))
        {
            visited.Add(task);

            foreach (Task dependency in task.Dependencies)
            {
                VisitTask(dependency, visited, sortedTasks);
            }

            sortedTasks.Add(task);
        }
    }

    public void GetEarliestTimes()
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
        catch (Exception error)
        {
            Console.WriteLine("Error saving earliest times: " + error.Message);
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

        int earliestTime = maxDependencyTime + task.Duration;
        earliestTimes[task] = earliestTime;
        return earliestTime;
    }
}