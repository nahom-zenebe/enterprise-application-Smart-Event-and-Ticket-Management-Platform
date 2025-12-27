public class RegisterUserCommandHandler{
    private readonly IUserRepository _repository;

   public RegisterUserCommandHandler(IUserRepository repository)
   {
    _repository=repository

   }

   public async Task<Guid>handle(RegisterUserCommand command)
   {
     var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
     var user = new User(command.Email, passwordHash, command.Role);

     await _repository.AddAsync(user);

     //publish user registere event via message broker here


      return user.Id;

   }

}