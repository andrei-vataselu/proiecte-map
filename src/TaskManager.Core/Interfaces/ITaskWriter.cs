using TaskManager.Core.Models;

namespace TaskManager.Core.Interfaces;

public interface ITaskWriter
{
    void Add(TaskItem task);
    void Update(TaskItem task);
    void Delete(int id);
}
