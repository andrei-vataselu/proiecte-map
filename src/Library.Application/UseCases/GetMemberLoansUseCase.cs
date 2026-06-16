using Library.Application.Abstractions;
using Library.Domain;

namespace Library.Application.UseCases;

public class GetMemberLoansUseCase
{
    private readonly ILoanRepository _loans;
    private readonly IMemberRepository _members;

    public GetMemberLoansUseCase(ILoanRepository loans, IMemberRepository members)
    {
        _loans = loans;
        _members = members;
    }

    public IReadOnlyList<Loan> Execute(Guid memberId)
    {
        if (_members.GetById(memberId) is null)
            throw new NotFoundException("Membru negasit");
        return _loans.GetActiveByMember(memberId);
    }
}
