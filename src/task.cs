class Task
{
    public string Id { get; set; }
    public int Duration { get; set; }
    public List<Task> Dependencies { get; }

    public Task(string id, int duration = 0)
    {
        Id = id;
        Duration = duration;
        Dependencies = new List<Task>();
    }

    public void AddDependency(Task task)
    {
        if (!Dependencies.Contains(task))
        {
            Dependencies.Add(task);
        }
    }

    public void RemoveDependency(string taskId) => Dependencies.RemoveAll(t => t.Id == taskId);
}