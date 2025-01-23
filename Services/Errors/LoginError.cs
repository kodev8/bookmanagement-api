namespace WebApplication1.Services.Errors
{
    public enum LoginError
    {
        None = 0,
        InvalidCredentials = 1
    }

    public static class LoginErrorExtensions
    {
        public static string GetMessage(this LoginError error)
        {
            return error switch
            {
                LoginError.None => "Success",
                LoginError.InvalidCredentials => "Invalid email or password",
                _ => "Unknown error"
            };
        }
    }
} 