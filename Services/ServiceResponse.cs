namespace WebApplication1.Services
{


    public class ServiceResponse<T, E>
    {
        public T? Data { get; set; }
        public E? Error { get; set; } = default(E);
    }
}