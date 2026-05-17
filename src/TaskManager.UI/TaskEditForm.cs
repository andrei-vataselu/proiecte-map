using TaskManager.Core.Models;

namespace TaskManager.UI;

public class TaskEditForm : Form
{
    private readonly TaskItem? _existing;
    private readonly TextBox _txtTitle;
    private readonly TextBox _txtDescription;
    private readonly ComboBox _cmbStatus;
    private readonly ComboBox _cmbPriority;
    private readonly ComboBox _cmbTaskType;
    private readonly ComboBox _cmbNotification;
    private readonly DateTimePicker _dtpDueDate;
    private readonly CheckBox _chkDueDate;
    private readonly NumericUpDown _numRecurrence;

    public TaskEditForm(TaskItem? existing = null)
    {
        _existing = existing;

        Text = existing == null ? "Sarcină nouă" : "Editare sarcină";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(480, 420);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 9,
            Padding = new Padding(12)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _txtTitle = new TextBox();
        _txtDescription = new TextBox { Multiline = true, Height = 60 };
        _cmbStatus = CreateEnumCombo<TaskItemStatus>();
        _cmbPriority = CreateEnumCombo<TaskPriority>();
        _cmbTaskType = CreateEnumCombo<TaskTypeKind>();
        _cmbNotification = CreateEnumCombo<NotificationType>();
        _chkDueDate = new CheckBox { Text = "Are dată limită" };
        _dtpDueDate = new DateTimePicker { Format = DateTimePickerFormat.Short, Enabled = false };
        _numRecurrence = new NumericUpDown { Minimum = 1, Maximum = 365, Value = 7 };

        _chkDueDate.CheckedChanged += (_, _) => _dtpDueDate.Enabled = _chkDueDate.Checked;
        _cmbTaskType.SelectedIndexChanged += (_, _) => UpdateFieldVisibility();

        AddRow(layout, "Titlu:", _txtTitle);
        AddRow(layout, "Descriere:", _txtDescription);
        AddRow(layout, "Status:", _cmbStatus);
        AddRow(layout, "Prioritate:", _cmbPriority);
        AddRow(layout, "Tip:", _cmbTaskType);
        AddRow(layout, "Notificare:", _cmbNotification);
        AddRow(layout, "Due date:", new Panel
        {
            Dock = DockStyle.Fill,
            Controls = { _chkDueDate, _dtpDueDate }
        });
        AddRow(layout, "Recurență (zile):", _numRecurrence);

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 48,
            Padding = new Padding(8)
        };
        var btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Anulează", DialogResult = DialogResult.Cancel };
        buttons.Controls.Add(btnOk);
        buttons.Controls.Add(btnCancel);

        AcceptButton = btnOk;
        CancelButton = btnCancel;

        Controls.Add(layout);
        Controls.Add(buttons);

        if (existing != null)
            LoadFromTask(existing);
        else
        {
            _cmbStatus.SelectedItem = TaskItemStatus.Todo;
            _cmbPriority.SelectedItem = TaskPriority.Medium;
            _cmbTaskType.SelectedItem = TaskTypeKind.Standard;
            _cmbNotification.SelectedItem = NotificationType.Console;
        }

        UpdateFieldVisibility();
    }

    private static ComboBox CreateEnumCombo<T>() where T : struct, Enum
    {
        var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
        foreach (var value in Enum.GetValues<T>())
            combo.Items.Add(value);
        return combo;
    }

    private static void AddRow(TableLayoutPanel layout, string label, Control control)
    {
        var row = layout.RowCount;
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row - 1);
        control.Dock = DockStyle.Fill;
        layout.Controls.Add(control, 1, row - 1);
        layout.RowCount++;
    }

    private void LoadFromTask(TaskItem task)
    {
        _txtTitle.Text = task.Title;
        _txtDescription.Text = task.Description ?? string.Empty;
        _cmbStatus.SelectedItem = task.Status;
        _cmbPriority.SelectedItem = task.Priority;
        _cmbTaskType.SelectedItem = task.TaskType;
        _cmbNotification.SelectedItem = task.NotificationType;

        if (task.DueDate.HasValue)
        {
            _chkDueDate.Checked = true;
            _dtpDueDate.Value = task.DueDate.Value.ToLocalTime();
        }

        if (task.RecurrenceInterval.HasValue)
            _numRecurrence.Value = task.RecurrenceInterval.Value;
    }

    private void UpdateFieldVisibility()
    {
        var type = (TaskTypeKind)(_cmbTaskType.SelectedItem ?? TaskTypeKind.Standard);
        _numRecurrence.Enabled = type == TaskTypeKind.Recurring;
        _chkDueDate.Enabled = type != TaskTypeKind.Standard;
        if (type == TaskTypeKind.Deadline)
            _chkDueDate.Checked = true;
    }

    public TaskItem BuildTask()
    {
        var type = (TaskTypeKind)_cmbTaskType.SelectedItem!;
        DateTime? dueDate = _chkDueDate.Checked
            ? _dtpDueDate.Value.ToUniversalTime()
            : null;

        int? recurrence = type == TaskTypeKind.Recurring
            ? (int)_numRecurrence.Value
            : null;

        var task = TaskItem.Create(
            type,
            _txtTitle.Text.Trim(),
            string.IsNullOrWhiteSpace(_txtDescription.Text) ? null : _txtDescription.Text.Trim(),
            (TaskItemStatus)_cmbStatus.SelectedItem!,
            (TaskPriority)_cmbPriority.SelectedItem!,
            (NotificationType)_cmbNotification.SelectedItem!,
            dueDate,
            recurrence);

        return task;
    }
}
