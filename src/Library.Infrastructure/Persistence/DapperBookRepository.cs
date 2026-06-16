using Dapper;
using Library.Application.Abstractions;
using Library.Domain;
using Microsoft.Data.Sqlite;

namespace Library.Infrastructure.Persistence;

public class DapperBookRepository : IBookRepository
{
    private readonly string _connectionString;

    public DapperBookRepository(string connectionString) => _connectionString = connectionString;

    public Book? GetById(Guid id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var row = connection.QuerySingleOrDefault<BookRow>(
            "SELECT Id, Title, AvailableCopies FROM Books WHERE Id = @Id",
            new { Id = id.ToString() });
        return row?.ToDomain();
    }

    public void Save(Book book)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Execute(@"
            INSERT INTO Books (Id, Title, AvailableCopies) VALUES (@Id, @Title, @AvailableCopies)
            ON CONFLICT(Id) DO UPDATE SET Title = @Title, AvailableCopies = @AvailableCopies",
            new { Id = book.Id.ToString(), book.Title, AvailableCopies = book.AvailableCopies });
    }

    private sealed class BookRow
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public int AvailableCopies { get; set; }

        public Book ToDomain() => new(Guid.Parse(Id), Title, AvailableCopies);
    }
}
