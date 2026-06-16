namespace Library.Domain;

public class Member
{
    public Guid Id { get; }
    public string Name { get; }

    public Member(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
