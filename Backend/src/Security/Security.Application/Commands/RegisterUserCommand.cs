


public class RegisterUserCommand
{
    public string Email { get; }
    public string Password { get; }
    public RoleType Role { get; }

    public RegisterUserCommand(string email, string password, RoleType role)
    {
        Email = email;
        Password = password;
        Role = role;
    }
}
