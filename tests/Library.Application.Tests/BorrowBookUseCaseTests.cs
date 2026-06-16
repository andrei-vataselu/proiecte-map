using Library.Application.Abstractions;
using Library.Application.UseCases;
using Library.Domain;

namespace Library.Application.Tests;

public class BorrowBookUseCaseTests
{
    [Fact]
    public void Execute_creates_loan_when_rules_allow()
    {
        var memberId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var books = new FakeBookRepository(new Book(bookId, "Test", 2));
        var loans = new FakeLoanRepository();
        var members = new FakeMemberRepository(new Member(memberId, "Test"));
        var useCase = new BorrowBookUseCase(books, loans, members);

        var loanId = useCase.Execute(memberId, bookId);

        Assert.NotEqual(Guid.Empty, loanId);
        Assert.Equal(1, loans.Count);
        Assert.Equal(1, books.GetById(bookId)!.AvailableCopies);
    }

    [Fact]
    public void Execute_throws_when_stock_empty()
    {
        var memberId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var books = new FakeBookRepository(new Book(bookId, "Test", 0));
        var loans = new FakeLoanRepository();
        var members = new FakeMemberRepository(new Member(memberId, "Test"));
        var useCase = new BorrowBookUseCase(books, loans, members);

        Assert.Throws<DomainException>(() => useCase.Execute(memberId, bookId));
    }

    [Fact]
    public void Execute_throws_when_member_has_five_active_loans()
    {
        var memberId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var books = new FakeBookRepository(new Book(bookId, "Test", 10));
        var loans = new FakeLoanRepository();
        for (var i = 0; i < 5; i++)
            loans.Save(new Loan(Guid.NewGuid(), memberId, Guid.NewGuid(), DateTime.UtcNow));
        var members = new FakeMemberRepository(new Member(memberId, "Test"));
        var useCase = new BorrowBookUseCase(books, loans, members);

        Assert.Throws<DomainException>(() => useCase.Execute(memberId, bookId));
    }

    private sealed class FakeBookRepository : IBookRepository
    {
        private readonly Dictionary<Guid, Book> _books = new();

        public FakeBookRepository(params Book[] books)
        {
            foreach (var book in books)
                _books[book.Id] = book;
        }

        public Book? GetById(Guid id) => _books.TryGetValue(id, out var book) ? book : null;

        public void Save(Book book) => _books[book.Id] = book;
    }

    private sealed class FakeLoanRepository : ILoanRepository
    {
        private readonly List<Loan> _loans = new();
        public int Count => _loans.Count;

        public Loan? GetById(Guid id) => _loans.FirstOrDefault(l => l.Id == id);

        public void Save(Loan loan)
        {
            _loans.RemoveAll(l => l.Id == loan.Id);
            _loans.Add(loan);
        }

        public int CountActiveByMember(Guid memberId) =>
            _loans.Count(l => l.MemberId == memberId && l.IsActive);

        public IReadOnlyList<Loan> GetActiveByMember(Guid memberId) =>
            _loans.Where(l => l.MemberId == memberId && l.IsActive).ToList();
    }

    private sealed class FakeMemberRepository : IMemberRepository
    {
        private readonly Dictionary<Guid, Member> _members = new();

        public FakeMemberRepository(params Member[] members)
        {
            foreach (var member in members)
                _members[member.Id] = member;
        }

        public Member? GetById(Guid id) => _members.TryGetValue(id, out var member) ? member : null;
    }
}
