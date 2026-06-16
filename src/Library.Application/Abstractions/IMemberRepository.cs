using Library.Domain;

namespace Library.Application.Abstractions;

public interface IMemberRepository
{
    Member? GetById(Guid id);
}
