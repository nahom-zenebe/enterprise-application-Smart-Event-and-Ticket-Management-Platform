


public class RegisterUserCommand
{
    public string Email { get; }
    public string Password { get; }
    public RoleType Role { get; }
    public RegisterUserCommand(string email, string password, string role)
    {
        Email = email;
        Password = password;
        Role = role;
    }
}
