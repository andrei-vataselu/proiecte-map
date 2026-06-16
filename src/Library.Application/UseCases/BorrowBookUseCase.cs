using Library.Application.Abstractions;
using Library.Domain;

namespace Library.Application.UseCases;

public class BorrowBookUseCase
{
    private readonly IBookRepository _books;
    private readonly ILoanRepository _loans;
    private readonly IMemberRepository _members;

    public BorrowBookUseCase(IBookRepository books, ILoanRepository loans, IMemberRepository members)
    {
        _books = books;
        _loans = loans;
        _members = members;
    }

    public Guid Execute(Guid memberId, Guid bookId)
    {
        if (_members.GetById(memberId) is null)
            throw new NotFoundException("Membru negasit");
        var book = _books.GetById(bookId);
        if (book is null)
            throw new NotFoundException("Carte negasita");
        var activeCount = _loans.CountActiveByMember(memberId);
        if (activeCount >= 5)
            throw new DomainException("Limita de imprumuturi active depasita");

        book.MarkAsBorrowed();
        var loan = new Loan(Guid.NewGuid(), memberId, bookId, DateTime.UtcNow);
        _books.Save(book);
        _loans.Save(loan);
        return loan.Id;
    }
}
