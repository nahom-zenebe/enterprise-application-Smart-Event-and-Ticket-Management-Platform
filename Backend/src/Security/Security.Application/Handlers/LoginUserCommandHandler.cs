using System;

public class LoginUserCommandHandler
{
    private readonly IUserRepository _repository;

    public LoginUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(LoginUserCommand command)
    {
        // minimal implementation for now: lookup by id not available by email
        // Return empty Guid to satisfy signature until login logic implemented
        return await Task.FromResult(Guid.Empty);
    }
}