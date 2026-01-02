


public enum RoleType
{
    Buyer,
    Seller,
    Admin
}

public class User
{
    public Guid Id { get; private set; }
    public string Name{get;private set;}
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public RoleType Role { get; private set; }

    private List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

    public User(string email, string passwordHash, RoleType role)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;

        _domainEvents.Add(new UserRegistered(this));
    }

    public void UpdateProfile(string email, RoleType role)
    {
        Email = email;
        Role = role;
    }
}
