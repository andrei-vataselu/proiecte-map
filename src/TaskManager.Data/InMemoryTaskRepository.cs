using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Data;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public IReadOnlyList<TaskItem> GetAll() =>
        _tasks.OrderByDescending(t => t.CreatedAt).ToList();

    public TaskItem? GetById(int id) =>
        _tasks.FirstOrDefault(t => t.Id == id);

    public void Add(TaskItem task)
    {
        task.Id = _nextId++;
        if (task.CreatedAt == default)
            task.CreatedAt = DateTime.UtcNow;

        _tasks.Add(CloneTask(task));
    }

    public void Update(TaskItem task)
    {
        var index = _tasks.FindIndex(t => t.Id == task.Id);
        if (index < 0)
            throw new KeyNotFoundException($"Task with id {task.Id} was not found.");

        _tasks[index] = CloneTask(task);
    }

    public void Delete(int id)
    {
        var index = _tasks.FindIndex(t => t.Id == id);
        if (index < 0)
            throw new KeyNotFoundException($"Task with id {id} was not found.");

        _tasks.RemoveAt(index);
    }

    private static TaskItem CloneTask(TaskItem source)
    {
        var clone = TaskItem.Create(
            source.TaskType,
            source.Title,
            source.Description,
            source.Status,
            source.Priority,
            source.NotificationType,
            source.DueDate,
            source.RecurrenceInterval);

        clone.Id = source.Id;
        clone.CreatedAt = source.CreatedAt;
        return clone;
    }
}
