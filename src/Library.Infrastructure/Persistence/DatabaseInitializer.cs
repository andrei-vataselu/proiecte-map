using Dapper;
using Microsoft.Data.Sqlite;

namespace Library.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Books (
                Id TEXT PRIMARY KEY,
                Title TEXT NOT NULL,
                AvailableCopies INTEGER NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Members (
                Id TEXT PRIMARY KEY,
                Name TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Loans (
                Id TEXT PRIMARY KEY,
                MemberId TEXT NOT NULL,
                BookId TEXT NOT NULL,
                BorrowedAt TEXT NOT NULL,
                DueDate TEXT NOT NULL,
                ReturnedAt TEXT NULL
            );");

        var count = connection.ExecuteScalar<long>("SELECT COUNT(*) FROM Books");
        if (count > 0)
            return;

        var memberId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var bookId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        connection.Execute(
            "INSERT INTO Members (Id, Name) VALUES (@Id, @Name)",
            new { Id = memberId.ToString(), Name = "Ana Pop" });
        connection.Execute(
            "INSERT INTO Books (Id, Title, AvailableCopies) VALUES (@Id, @Title, @AvailableCopies)",
            new { Id = bookId.ToString(), Title = "Clean Code", AvailableCopies = 3 });
    }
}
