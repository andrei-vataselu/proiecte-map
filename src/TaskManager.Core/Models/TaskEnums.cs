namespace TaskManager.Core.Models;

public enum TaskItemStatus
{
    Todo,
    InProgress,
    Done
}

public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3
}

public enum TaskTypeKind
{
    Standard,
    Recurring,
    Deadline
}

public enum NotificationType
{
    Email,
    Console,
    FileLog,
    Slack
}
