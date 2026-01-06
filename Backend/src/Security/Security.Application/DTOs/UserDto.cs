public class RegisterDto{
   public string Name{get; set;}
   public string Email { get; set; }
   public RoleType Role { get; set; }
   public string PasswordHash { get; private set; }
}

public class LoginDto{
   public string Email { get; set; }
   public string PasswordHash { get; private set; }
}

public class ResponseUserDto{
   public string Name{get; set;}
   public string Email { get; set; }
   public RoleType Role { get; set; }
}