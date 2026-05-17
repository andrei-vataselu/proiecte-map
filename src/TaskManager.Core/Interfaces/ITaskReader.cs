using TaskManager.Core.Models;

namespace TaskManager.Core.Interfaces;

public interface ITaskReader
{
    IReadOnlyList<TaskItem> GetAll();
    TaskItem? GetById(int id);
}
