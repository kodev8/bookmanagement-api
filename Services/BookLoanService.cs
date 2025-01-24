using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Contexts;
using WebApplication1.Models;
using WebApplication1.Services.Errors;
using System.Linq;

namespace WebApplication1.Services
{
    public class BookLoanService
    {
        private readonly BookDbContext _context;
        private readonly UserDataService _userService;

        public BookLoanService(BookDbContext context, UserDataService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<BookLoanDTOOut?> GetBookLoan(int id)
        {
            BookLoan? bookLoan = await _context.BookLoans.FindAsync(id);
            if (bookLoan == null) return null;
            return ToDTO(bookLoan);
        }

        public async Task<IEnumerable<BookLoanDTOOut>> GetAllLoans()
        {
            return await _context.BookLoans.Select(bl => ToDTO(bl)).ToListAsync();
        }

        public async Task<IEnumerable<BookLoanDTOOut>> GetUserLoans(string userId)
        {
            return await _context.BookLoans.Where(bl => bl.UserId == userId).Select(bl => ToDTO(bl)).ToListAsync();
        }

        public async Task<ServiceResponse<BookLoanDTOOut, BookLoanError>> AddBookLoan(BookLoanDTOIn bookLoanDTO)
        {
            Book? book = await _context.Books.FindAsync(bookLoanDTO.BookId);

            // check if we found the book
            if (book == null || book.IsRemoved)
                return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.BookNotFound };

            // check if book is available in our library
            if (book.AvailableCopies <= 0)
                return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.BookNotAvailable };

            // check if user already has a loan for/borrowed this book
            if (await _context.BookLoans.AnyAsync(bl => bl.UserId == bookLoanDTO.UserId && bl.BookId == bookLoanDTO.BookId)) return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.AlreadyHasLoan };

            // check if user has overdue loans (don't allow borrowing if they have one)
            if (await _context.BookLoans.AnyAsync(bl => bl.UserId == bookLoanDTO.UserId && bl.ReturnDate < DateTime.Now)) return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.OverdueLoans };

            // check if user is found
            if (await _userService.GetUser(bookLoanDTO.UserId!) == null) return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.UserNotFound };

            // check if user has fines
            if (await _context.Fines.AnyAsync(f => f.UserId == bookLoanDTO.UserId && f.Status == EFineStatus.Pending.ToString())) return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.UserHasFines };

            // Allow borrorw for 14 days
            BookLoan bookLoan = new BookLoan
            {
                BookId = bookLoanDTO.BookId,
                UserId = bookLoanDTO.UserId,
                DueDate = DateTime.Now.AddDays(14),
                ReturnDate = null,
                Status = EStatus.Active.ToString(),
                Notes = string.Empty
            };

            book.AvailableCopies--;
            await _context.SaveChangesAsync();

            await _context.BookLoans.AddAsync(bookLoan);
            await _context.SaveChangesAsync();
            return new ServiceResponse<BookLoanDTOOut, BookLoanError>
            {
                Data = ToDTO(bookLoan),
                Error = BookLoanError.None
            };
        }

        public async Task<ServiceResponse<BookLoanDTOOut, BookLoanError>> ReturnBookLoan(int loanId, string userId)
        {

            // check if user exists
            if (await _userService.GetUser(userId) == null) return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.UserNotFound };

            BookLoan? bookLoan = await _context.BookLoans.FindAsync(loanId);
            if (bookLoan == null) return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.BookLoanNotFound };
            // check if user has the book loaned
            if (bookLoan.UserId != userId) return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.UserDoesNotHaveLoan };

            // check if book is already returned
            if (bookLoan.ReturnDate != null && bookLoan.Status == EStatus.Returned.ToString()) return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.BookAlreadyReturned };

            
            // check if book is overdue and create a fine
            if (bookLoan.DueDate < DateTime.Now)
            {
                Fine fine = new Fine
                {
                    BookLoanId = loanId,
                    UserId = userId,
                    Amount = 10,
                    Reason = "Book overdue",
                    IssuedDate = DateTime.Now,
                    Status = EFineStatus.Pending.ToString()
                };
                await _context.Fines.AddAsync(fine);
                await _context.SaveChangesAsync();
                return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.BookOverdue };
            }

            bookLoan.ReturnDate = DateTime.Now;
            bookLoan.Status = EStatus.Returned.ToString();
            Book? book = await _context.Books.FindAsync(bookLoan.BookId);
            if (book != null)
            {
                book.AvailableCopies++;
                await _context.SaveChangesAsync();
            }
            return new ServiceResponse<BookLoanDTOOut, BookLoanError>
            {
                Data = ToDTO(bookLoan),
                Error = BookLoanError.None
            };
        }

        public async Task<ServiceResponse<BookLoanDTOOut, BookLoanError>> UpdateBookLoanStatus(int id, EStatus status)
        {
            BookLoan? bookLoan = await _context.BookLoans.FindAsync(id);
            if (bookLoan == null) return new ServiceResponse<BookLoanDTOOut, BookLoanError> { Error = BookLoanError.BookLoanNotFound };
            bookLoan.Status = status.ToString();
            await _context.SaveChangesAsync();
            return new ServiceResponse<BookLoanDTOOut, BookLoanError>
            {
                Data = ToDTO(bookLoan),
                Error = BookLoanError.None
            };
        }

        private static BookLoanDTOOut ToDTO(BookLoan bookLoan)
        {
            return new BookLoanDTOOut(
                bookLoan.Id,
                bookLoan.BookId,
                bookLoan.UserId!,
                bookLoan.DueDate,
                bookLoan.ReturnDate,
                Enum.Parse<EStatus>(bookLoan.Status),
                bookLoan.Notes
            );
        }


    }
}
