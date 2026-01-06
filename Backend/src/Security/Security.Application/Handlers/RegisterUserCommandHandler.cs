using System;
using System.Security.Cryptography;
using System.Text;

public class RegisterUserCommandHandler
{
   private readonly IUserRepository _repository;

   public RegisterUserCommandHandler(IUserRepository repository)
   {
      _repository = repository;
   }

   public async Task<Guid> Handle(RegisterUserCommand command)
   {
      var passwordHash = HashPassword(command.Password);
      var user = new User(command.Email, passwordHash, command.Role);

      await _repository.AddAsync(user);

      // publish user registered event via message broker here

      return user.Id;
   }

   private static string HashPassword(string password)
   {
      using var sha = SHA256.Create();
      var bytes = Encoding.UTF8.GetBytes(password);
      var hash = sha.ComputeHash(bytes);
      var sb = new StringBuilder();
      foreach (var b in hash) sb.Append(b.ToString("x2"));
      return sb.ToString();
   }
}