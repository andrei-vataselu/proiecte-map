using Library.Application.UseCases;
using Library.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers;

[ApiController]
[Route("loans")]
public class LoansController : ControllerBase
{
    private readonly BorrowBookUseCase _borrow;
    private readonly ReturnBookUseCase _returnBook;
    private readonly GetMemberLoansUseCase _getMemberLoans;

    public LoansController(
        BorrowBookUseCase borrow,
        ReturnBookUseCase returnBook,
        GetMemberLoansUseCase getMemberLoans)
    {
        _borrow = borrow;
        _returnBook = returnBook;
        _getMemberLoans = getMemberLoans;
    }

    [HttpPost("borrow")]
    public IActionResult Borrow([FromBody] BorrowRequest request)
    {
        try
        {
            var loanId = _borrow.Execute(request.MemberId, request.BookId);
            return Ok(new { loanId });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{loanId:guid}/return")]
    public IActionResult Return(Guid loanId)
    {
        try
        {
            var penalty = _returnBook.Execute(loanId);
            return Ok(new { penalty });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("member/{memberId:guid}")]
    public IActionResult GetMemberLoans(Guid memberId)
    {
        try
        {
            var loans = _getMemberLoans.Execute(memberId);
            return Ok(loans.Select(l => new
            {
                l.Id,
                l.BookId,
                l.BorrowedAt,
                l.DueDate,
                l.IsActive
            }));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}

public record BorrowRequest(Guid MemberId, Guid BookId);
