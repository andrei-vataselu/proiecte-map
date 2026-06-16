namespace Library.Domain;

public class Book
{
    public Guid Id { get; }
    public string Title { get; }
    public int AvailableCopies { get; private set; }

    public Book(Guid id, string title, int availableCopies)
    {
        Id = id;
        Title = title;
        AvailableCopies = availableCopies;
    }

    public bool CanBeBorrowed() => AvailableCopies > 0;

    public void MarkAsBorrowed()
    {
        if (!CanBeBorrowed())
            throw new DomainException("Stoc epuizat");
        AvailableCopies--;
    }

    public void MarkAsReturned() => AvailableCopies++;
}
