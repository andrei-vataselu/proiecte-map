using Dapper;
using Library.Application.Abstractions;
using Library.Domain;
using Microsoft.Data.Sqlite;

namespace Library.Infrastructure.Persistence;

public class DapperMemberRepository : IMemberRepository
{
    private readonly string _connectionString;

    public DapperMemberRepository(string connectionString) => _connectionString = connectionString;

    public Member? GetById(Guid id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var row = connection.QuerySingleOrDefault<MemberRow>(
            "SELECT Id, Name FROM Members WHERE Id = @Id",
            new { Id = id.ToString() });
        return row is null ? null : new Member(Guid.Parse(row.Id), row.Name);
    }

    private sealed class MemberRow
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
