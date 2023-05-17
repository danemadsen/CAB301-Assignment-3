using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Task
{
    public string Id { get; }
    public int TimeNeeded { get; set; }
    public List<Task> Dependencies { get; }

    public Task(string id, int timeNeeded = 0)
    {
        Id = id;
        TimeNeeded = timeNeeded;
        Dependencies = new List<Task>();
    }

    public void AddDependency(Task task)
    {
        if (!Dependencies.Contains(task))
        {
            Dependencies.Add(task);
        }
    }

    public void RemoveDependency(string taskId)
    {
        Dependencies.RemoveAll(t => t.Id == taskId);
    }
}