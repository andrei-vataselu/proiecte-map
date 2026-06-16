using Library.Application.Abstractions;
using Library.Domain;

namespace Library.Application.UseCases;

public class ReturnBookUseCase
{
    private readonly IBookRepository _books;
    private readonly ILoanRepository _loans;

    public ReturnBookUseCase(IBookRepository books, ILoanRepository loans)
    {
        _books = books;
        _loans = loans;
    }

    public decimal Execute(Guid loanId)
    {
        var loan = _loans.GetById(loanId);
        if (loan is null)
            throw new NotFoundException("Imprumut negasit");
        var book = _books.GetById(loan.BookId);
        if (book is null)
            throw new NotFoundException("Carte negasita");
        var now = DateTime.UtcNow;
        var penalty = loan.CalculatePenalty(now);
        loan.MarkAsReturned(now);
        book.MarkAsReturned();
        _loans.Save(loan);
        _books.Save(book);
        return penalty;
    }
}
