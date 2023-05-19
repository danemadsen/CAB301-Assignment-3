class TaskManager
{
    private Dictionary<string, Task> tasks;

    public TaskManager()
    {
        tasks = new Dictionary<string, Task>();
    }

    public void LoadTasks(string fileName)
    {
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

    public void SaveTasks(string fileName)
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
    }

    public void AddTask(string taskId, int Duration, List<string> dependencyIds)
    {
        if (tasks.ContainsKey(taskId)) throw new Exception("Task already exists!");
        tasks.Add(taskId, new Task(taskId, Duration));
        foreach (string Id in dependencyIds)
        {
            if (string.IsNullOrEmpty(Id) && string.IsNullOrWhiteSpace(Id)) break;
            if (tasks.ContainsKey(Id)) tasks[taskId].AddDependency(tasks[Id]);               
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

    public void SetDuration(string taskId, int Duration)
    {
        if (tasks.ContainsKey(taskId))
        {
            tasks[taskId].Duration = Duration;
            Console.WriteLine("Time needed changed successfully!");
        }
        else
        {
            Console.WriteLine("Task not found!");
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