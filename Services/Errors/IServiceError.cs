namespace WebApplication1.Services.Errors
{
    public interface IServiceError
    {
        int Code { get; }
        string Message { get; }
    }
} 