namespace WebApplication1.Services.Errors
{
    public enum BookError
    {
        None = 0,
        AuthorNotFound = 1,
        BookNotFound = 2,
        BookAlreadyExists = 3
    }

    public static class BookErrorExtensions
    {
        public static string GetMessage(this BookError error)
        {
            return error switch
            {
                BookError.None => "Success",
                BookError.AuthorNotFound => "One or more authors were not found",
                BookError.BookNotFound => "Book not found",
                BookError.BookAlreadyExists => "A book with this ISBN already exists",
                _ => "Unknown error"
            };
        }
    }
} 