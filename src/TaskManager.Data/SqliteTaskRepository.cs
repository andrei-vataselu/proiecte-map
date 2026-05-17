using Microsoft.Data.Sqlite;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
namespace TaskManager.Data;

public class SqliteTaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public SqliteTaskRepository(string? databasePath = null)
    {
        var path = databasePath ?? Path.Combine(AppContext.BaseDirectory, "tasks.db");
        _connectionString = new SqliteConnectionStringBuilder { DataSource = path }.ConnectionString;
        EnsureSchema();
    }

    private void EnsureSchema()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            @"CREATE TABLE IF NOT EXISTS Tasks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT,
                Status TEXT NOT NULL,
                Priority INTEGER NOT NULL,
                TaskType TEXT NOT NULL,
                NotificationType TEXT NOT NULL,
                DueDate TEXT,
                RecurrenceInterval INTEGER,
                CreatedAt TEXT NOT NULL
            );";
        command.ExecuteNonQuery();
    }

    public IReadOnlyList<TaskItem> GetAll()
    {
        var tasks = new List<TaskItem>();
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Tasks ORDER BY datetime(CreatedAt) DESC";

        using var reader = command.ExecuteReader();
        while (reader.Read())
            tasks.Add(MapFromReader(reader));

        return tasks;
    }

    public TaskItem? GetById(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Tasks WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? MapFromReader(reader) : null;
    }

    public void Add(TaskItem task)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            @"INSERT INTO Tasks (Title, Description, Status, Priority, TaskType, NotificationType, DueDate, RecurrenceInterval, CreatedAt)
              VALUES ($title, $description, $status, $priority, $taskType, $notificationType, $dueDate, $recurrenceInterval, $createdAt);";
        BindTaskParameters(command, task);
        command.ExecuteNonQuery();

        using var idCommand = connection.CreateCommand();
        idCommand.CommandText = "SELECT last_insert_rowid();";
        task.Id = Convert.ToInt32(idCommand.ExecuteScalar());
    }

    public void Update(TaskItem task)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            @"UPDATE Tasks SET
                Title = $title,
                Description = $description,
                Status = $status,
                Priority = $priority,
                TaskType = $taskType,
                NotificationType = $notificationType,
                DueDate = $dueDate,
                RecurrenceInterval = $recurrenceInterval,
                CreatedAt = $createdAt
              WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", task.Id);
        BindTaskParameters(command, task);
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Tasks WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    private SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private static void BindTaskParameters(SqliteCommand command, TaskItem task)
    {
        if (task.CreatedAt == default)
            task.CreatedAt = DateTime.UtcNow;

        command.Parameters.AddWithValue("$title", task.Title);
        command.Parameters.AddWithValue("$description", (object?)task.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("$status", task.Status.ToString());
        command.Parameters.AddWithValue("$priority", (int)task.Priority);
        command.Parameters.AddWithValue("$taskType", task.TaskType.ToString());
        command.Parameters.AddWithValue("$notificationType", task.NotificationType.ToString());
        command.Parameters.AddWithValue("$dueDate", task.DueDate.HasValue ? task.DueDate.Value.ToString("O") : DBNull.Value);
        command.Parameters.AddWithValue("$recurrenceInterval", task.RecurrenceInterval.HasValue ? task.RecurrenceInterval.Value : DBNull.Value);
        command.Parameters.AddWithValue("$createdAt", task.CreatedAt.ToString("O"));
    }

    private static TaskItem MapFromReader(SqliteDataReader reader)
    {
        var taskType = Enum.Parse<TaskTypeKind>(reader.GetString(reader.GetOrdinal("TaskType")));
        var title = reader.GetString(reader.GetOrdinal("Title"));
        var description = reader.IsDBNull(reader.GetOrdinal("Description"))
            ? null
            : reader.GetString(reader.GetOrdinal("Description"));
        var status = Enum.Parse<TaskItemStatus>(reader.GetString(reader.GetOrdinal("Status")));
        var priority = (TaskPriority)reader.GetInt32(reader.GetOrdinal("Priority"));
        var notificationType = Enum.Parse<NotificationType>(reader.GetString(reader.GetOrdinal("NotificationType")));

        DateTime? dueDate = reader.IsDBNull(reader.GetOrdinal("DueDate"))
            ? null
            : DateTime.Parse(reader.GetString(reader.GetOrdinal("DueDate")));

        int? recurrenceInterval = reader.IsDBNull(reader.GetOrdinal("RecurrenceInterval"))
            ? null
            : reader.GetInt32(reader.GetOrdinal("RecurrenceInterval"));

        var task = TaskItem.Create(taskType, title, description, status, priority, notificationType, dueDate, recurrenceInterval);
        task.Id = reader.GetInt32(reader.GetOrdinal("Id"));
        task.CreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedAt")));

        return task;
    }
}
