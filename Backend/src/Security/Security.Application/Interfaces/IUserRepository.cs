

public class IUserRepository{
    Task Register(User user);
    Task <User>GetUserbyId(int id);
    Task Login(User user);

}