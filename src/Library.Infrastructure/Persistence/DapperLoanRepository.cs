using Dapper;
using Library.Application.Abstractions;
using Library.Domain;
using Microsoft.Data.Sqlite;

namespace Library.Infrastructure.Persistence;

public class DapperLoanRepository : ILoanRepository
{
    private readonly string _connectionString;

    public DapperLoanRepository(string connectionString) => _connectionString = connectionString;

    public Loan? GetById(Guid id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var row = connection.QuerySingleOrDefault<LoanRow>(
            "SELECT Id, MemberId, BookId, BorrowedAt, DueDate, ReturnedAt FROM Loans WHERE Id = @Id",
            new { Id = id.ToString() });
        return row?.ToDomain();
    }

    public void Save(Loan loan)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Execute(@"
            INSERT INTO Loans (Id, MemberId, BookId, BorrowedAt, DueDate, ReturnedAt)
            VALUES (@Id, @MemberId, @BookId, @BorrowedAt, @DueDate, @ReturnedAt)
            ON CONFLICT(Id) DO UPDATE SET
                MemberId = @MemberId,
                BookId = @BookId,
                BorrowedAt = @BorrowedAt,
                DueDate = @DueDate,
                ReturnedAt = @ReturnedAt",
            new
            {
                Id = loan.Id.ToString(),
                MemberId = loan.MemberId.ToString(),
                BookId = loan.BookId.ToString(),
                BorrowedAt = loan.BorrowedAt.ToString("O"),
                DueDate = loan.DueDate.ToString("O"),
                ReturnedAt = loan.ReturnedAt?.ToString("O")
            });
    }

    public int CountActiveByMember(Guid memberId)
    {
        using var connection = new SqliteConnection(_connectionString);
        return connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Loans WHERE MemberId = @MemberId AND ReturnedAt IS NULL",
            new { MemberId = memberId.ToString() });
    }

    public IReadOnlyList<Loan> GetActiveByMember(Guid memberId)
    {
        using var connection = new SqliteConnection(_connectionString);
        var rows = connection.Query<LoanRow>(
            "SELECT Id, MemberId, BookId, BorrowedAt, DueDate, ReturnedAt FROM Loans WHERE MemberId = @MemberId AND ReturnedAt IS NULL",
            new { MemberId = memberId.ToString() });
        return rows.Select(r => r.ToDomain()).ToList();
    }

    private sealed class LoanRow
    {
        public string Id { get; set; } = "";
        public string MemberId { get; set; } = "";
        public string BookId { get; set; } = "";
        public string BorrowedAt { get; set; } = "";
        public string DueDate { get; set; } = "";
        public string? ReturnedAt { get; set; }

        public Loan ToDomain()
        {
            var loan = new Loan(
                Guid.Parse(Id),
                Guid.Parse(MemberId),
                Guid.Parse(BookId),
                DateTime.Parse(BorrowedAt, null, System.Globalization.DateTimeStyles.RoundtripKind),
                (DateTime.Parse(DueDate, null, System.Globalization.DateTimeStyles.RoundtripKind) -
                 DateTime.Parse(BorrowedAt, null, System.Globalization.DateTimeStyles.RoundtripKind)).Days);

            if (ReturnedAt is not null)
                loan.MarkAsReturned(DateTime.Parse(ReturnedAt, null, System.Globalization.DateTimeStyles.RoundtripKind));

            return loan;
        }
    }
}
