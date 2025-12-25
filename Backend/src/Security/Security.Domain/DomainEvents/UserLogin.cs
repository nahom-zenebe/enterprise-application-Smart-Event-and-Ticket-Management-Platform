public class UserLogin : DomainEvent
{
    public User User { get; }
    public UserLogin(User user) => User = user;
}

