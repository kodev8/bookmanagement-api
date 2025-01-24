namespace WebApplication1.Services.Errors
{
    public enum BookLoanError
    {
        None = 0,
        BookNotFound = 1,
        BookNotAvailable = 2,
        UserNotFound = 3,
        UserHasFines = 4,
        AlreadyHasLoan = 5,
        OverdueLoans = 6,
        UserDoesNotHaveLoan = 7,
        BookLoanNotFound = 8,
        BookAlreadyReturned = 9,
        BookOverdue = 10,
        FineNotFound = 11,
        FineAlreadyPaid = 12,
        FineOverdue = 13,
        FinePaid = 14,
        FineAlreadyIssued = 15
    }

    public static class BookLoanErrorExtensions
    {
        public static string GetMessage(this BookLoanError error)
        {
            return error switch
            {
                BookLoanError.None => "Success",
                BookLoanError.BookNotFound => "Book not found",
                BookLoanError.BookNotAvailable => "Book is not available for loan",
                BookLoanError.UserNotFound => "User not found",
                BookLoanError.UserHasFines => "User has pending fines",
                BookLoanError.AlreadyHasLoan => "User already has this book on loan",
                BookLoanError.OverdueLoans => "User has overdue loans",
                BookLoanError.UserDoesNotHaveLoan => "User does not have this book on loan",
                BookLoanError.BookLoanNotFound => "Book loan not found",
                BookLoanError.BookAlreadyReturned => "Book has already been returned",
                BookLoanError.BookOverdue => "Book is overdue",
                BookLoanError.FineNotFound => "Fine not found",
                BookLoanError.FineAlreadyPaid => "Fine has already been paid",
                BookLoanError.FineOverdue => "Fine is overdue",
                BookLoanError.FinePaid => "Fine has been paid",
                BookLoanError.FineAlreadyIssued => "Fine has already been issued",
                _ => "Unknown error"
            };
        }
    }
} 