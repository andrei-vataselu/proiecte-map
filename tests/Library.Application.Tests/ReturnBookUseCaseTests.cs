using Library.Application.Abstractions;
using Library.Application.UseCases;
using Library.Domain;

namespace Library.Application.Tests;

public class ReturnBookUseCaseTests
{
    [Fact]
    public void Execute_returns_penalty_for_late_return()
    {
        var memberId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var book = new Book(bookId, "Test", 0);
        var loan = new Loan(Guid.NewGuid(), memberId, bookId, DateTime.UtcNow.AddDays(-20));
        var books = new FakeBookRepository(book);
        var loans = new FakeLoanRepository(loan);
        var useCase = new ReturnBookUseCase(books, loans);

        var penalty = useCase.Execute(loan.Id);

        Assert.True(penalty > 0);
        Assert.Equal(1, books.GetById(bookId)!.AvailableCopies);
        Assert.False(loans.GetById(loan.Id)!.IsActive);
    }

    private sealed class FakeBookRepository : IBookRepository
    {
        private readonly Dictionary<Guid, Book> _books = new();

        public FakeBookRepository(Book book) => _books[book.Id] = book;

        public Book? GetById(Guid id) => _books.TryGetValue(id, out var book) ? book : null;

        public void Save(Book book) => _books[book.Id] = book;
    }

    private sealed class FakeLoanRepository : ILoanRepository
    {
        private readonly Dictionary<Guid, Loan> _loans = new();

        public FakeLoanRepository(Loan loan) => _loans[loan.Id] = loan;

        public Loan? GetById(Guid id) => _loans.TryGetValue(id, out var loan) ? loan : null;

        public void Save(Loan loan) => _loans[loan.Id] = loan;

        public int CountActiveByMember(Guid memberId) =>
            _loans.Values.Count(l => l.MemberId == memberId && l.IsActive);

        public IReadOnlyList<Loan> GetActiveByMember(Guid memberId) =>
            _loans.Values.Where(l => l.MemberId == memberId && l.IsActive).ToList();
    }
}
