using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Services;

public class ReportService
{
    private readonly ITaskReader _reader;

    public ReportService(ITaskReader reader)
    {
        _reader = reader;
    }

    public string GenerateSummary()
    {
        var tasks = _reader.GetAll();
        var total = tasks.Count;
        var done = tasks.Count(t => t.Status == TaskItemStatus.Done);
        return $"Total sarcini: {total}, finalizate (Done): {done}";
    }
}
