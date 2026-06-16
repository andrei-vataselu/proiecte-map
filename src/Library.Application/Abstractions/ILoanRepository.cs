using Library.Domain;

namespace Library.Application.Abstractions;

public interface ILoanRepository
{
    Loan? GetById(Guid id);
    void Save(Loan loan);
    int CountActiveByMember(Guid memberId);
    IReadOnlyList<Loan> GetActiveByMember(Guid memberId);
}
