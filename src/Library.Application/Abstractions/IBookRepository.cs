using Library.Domain;

namespace Library.Application.Abstractions;

public interface IBookRepository
{
    Book? GetById(Guid id);
    void Save(Book book);
}
