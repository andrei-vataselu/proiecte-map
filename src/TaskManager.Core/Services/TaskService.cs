using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Services;

public class TaskService
{
    private readonly ITaskRepository _repository;
    private readonly TaskValidator _validator;
    private readonly IReadOnlyDictionary<NotificationType, ITaskNotifier> _notifiers;

    public TaskService(
        ITaskRepository repository,
        TaskValidator validator,
        IReadOnlyDictionary<NotificationType, ITaskNotifier> notifiers)
    {
        _repository = repository;
        _validator = validator;
        _notifiers = notifiers;
    }

    public IReadOnlyList<TaskItem> GetAll() => _repository.GetAll();

    public TaskItem? GetById(int id) => _repository.GetById(id);

    public IReadOnlyList<TaskItem> GetFiltered(TaskItemStatus? status, TaskPriority? priority)
    {
        IEnumerable<TaskItem> query = _repository.GetAll();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        return query.ToList();
    }

    public void Add(TaskItem task)
    {
        _validator.ValidateOrThrow(task);
        _repository.Add(task);
    }

    public void Update(TaskItem task)
    {
        _validator.ValidateOrThrow(task);
        _repository.Update(task);
    }

    public void Delete(int id) => _repository.Delete(id);

    public void CompleteTask(int id)
    {
        var task = _repository.GetById(id)
            ?? throw new KeyNotFoundException($"Task with id {id} was not found.");

        task.Complete();
        _repository.Update(task);
        Notify(task);
    }

    private void Notify(TaskItem task)
    {
        if (_notifiers.TryGetValue(task.NotificationType, out var notifier))
            notifier.Notify(task);
    }
}
