public class LoginUserCommandHandler{

    private readonly IUserRepository _repository;


   public LoginUserCommandHandler(IUserRepository repository){
    _repository=repository;

   }

   public async Task<Guid> handle(LoginUserCommand command){
    //implmenting the login
   }
}