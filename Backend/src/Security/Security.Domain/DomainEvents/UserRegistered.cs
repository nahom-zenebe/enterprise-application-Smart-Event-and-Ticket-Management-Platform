public class UserRegistered : DomainEvent
{
    public User User { get; }
    public UserRegistered(User user) => User = user;
}

