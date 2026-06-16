namespace Library.Domain;

public class Loan
{
    public const int DefaultLoanDays = 14;
    public const decimal PenaltyPerDay = 2m;

    public Guid Id { get; }
    public Guid MemberId { get; }
    public Guid BookId { get; }
    public DateTime BorrowedAt { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; private set; }

    public bool IsActive => ReturnedAt == null;

    public Loan(Guid id, Guid memberId, Guid bookId, DateTime borrowedAt, int loanDays = DefaultLoanDays)
    {
        Id = id;
        MemberId = memberId;
        BookId = bookId;
        BorrowedAt = borrowedAt;
        DueDate = borrowedAt.AddDays(loanDays);
    }

    public void MarkAsReturned(DateTime when)
    {
        if (!IsActive)
            throw new DomainException("Imprumutul este deja returnat");
        ReturnedAt = when;
    }

    public decimal CalculatePenalty(DateTime returnedAt)
    {
        if (returnedAt <= DueDate)
            return 0;
        var days = (returnedAt.Date - DueDate.Date).Days;
        return days * PenaltyPerDay;
    }
}
