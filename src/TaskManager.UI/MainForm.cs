using TaskManager.Core.Models;
using TaskManager.Core.Services;

namespace TaskManager.UI;

public partial class MainForm : Form
{
    private readonly TaskService _taskService;
    private readonly ReportService _reportService;

    public MainForm(TaskService taskService, ReportService reportService)
    {
        _taskService = taskService;
        _reportService = reportService;
        InitializeComponent();
        InitializeFilters();
        LoadTasks();
    }

    private void InitializeFilters()
    {
        cmbStatusFilter.Items.Add("(Toate)");
        foreach (TaskItemStatus status in Enum.GetValues<TaskItemStatus>())
            cmbStatusFilter.Items.Add(status);

        cmbPriorityFilter.Items.Add("(Toate)");
        foreach (TaskPriority priority in Enum.GetValues<TaskPriority>())
            cmbPriorityFilter.Items.Add(priority);

        cmbStatusFilter.SelectedIndex = 0;
        cmbPriorityFilter.SelectedIndex = 0;
    }

    private void LoadTasks()
    {
        TaskItemStatus? status = cmbStatusFilter.SelectedIndex > 0
            ? (TaskItemStatus)cmbStatusFilter.SelectedItem!
            : null;

        TaskPriority? priority = cmbPriorityFilter.SelectedIndex > 0
            ? (TaskPriority)cmbPriorityFilter.SelectedItem!
            : null;

        var tasks = _taskService.GetFiltered(status, priority);
        gridTasks.DataSource = tasks
            .Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString(),
                Type = t.TaskType.ToString(),
                Notification = t.NotificationType.ToString(),
                DueDate = t.DueDate?.ToLocalTime().ToString("g") ?? "",
                Recurrence = t.RecurrenceInterval?.ToString() ?? "",
                Created = t.CreatedAt.ToLocalTime().ToString("g")
            })
            .ToList();
    }

    private int? SelectedTaskId()
    {
        if (gridTasks.CurrentRow == null)
            return null;

        return Convert.ToInt32(gridTasks.CurrentRow.Cells["Id"].Value);
    }

    private void BtnRefresh_Click(object? sender, EventArgs e) => LoadTasks();

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        using var form = new TaskEditForm();
        if (form.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            _taskService.Add(form.BuildTask());
            LoadTasks();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnEdit_Click(object? sender, EventArgs e)
    {
        var id = SelectedTaskId();
        if (!id.HasValue)
        {
            MessageBox.Show("Selectați o sarcină.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var task = _taskService.GetById(id.Value);
        if (task == null)
            return;

        using var form = new TaskEditForm(task);
        if (form.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            var updated = form.BuildTask();
            updated.Id = task.Id;
            updated.CreatedAt = task.CreatedAt;
            _taskService.Update(updated);
            LoadTasks();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        var id = SelectedTaskId();
        if (!id.HasValue)
        {
            MessageBox.Show("Selectați o sarcină.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (MessageBox.Show("Ștergeți sarcina selectată?", "Confirmare",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        try
        {
            _taskService.Delete(id.Value);
            LoadTasks();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnReport_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(
            _reportService.GenerateSummary(),
            "Raport sarcini",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void BtnComplete_Click(object? sender, EventArgs e)
    {
        var id = SelectedTaskId();
        if (!id.HasValue)
        {
            MessageBox.Show("Selectați o sarcină.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            _taskService.CompleteTask(id.Value);
            LoadTasks();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
