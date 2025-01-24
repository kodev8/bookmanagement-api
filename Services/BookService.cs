using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Contexts;
using WebApplication1.Models;
using WebApplication1.Services.Errors;

namespace WebApplication1.Services
{
    public class BookService
    {
        private readonly BookDbContext _context;

        public BookService(BookDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<BookDTOOut, BookError>> AddBook(BookDTOIn bookDTO)
        {
            // Check for existing book
            var foundBook = await _context.Books.FirstOrDefaultAsync(b => b.Isbn == bookDTO.Isbn);
            if (foundBook != null)
            {
                return new ServiceResponse<BookDTOOut, BookError>
                {
                    Data = null,
                    Error = BookError.BookAlreadyExists
                };
            }

            // Validate authors
            var foundAuthors = await _context.Authors.Where(a => bookDTO.Authors!.Contains(a.Id)).ToListAsync();
            var notFoundAuthors = bookDTO.Authors!.Where(a => !foundAuthors.Any(b => b.Id == a)).ToList();
            if (notFoundAuthors.Count > 0)
            {
                return new ServiceResponse<BookDTOOut, BookError>
                {
                    Data = null,
                    Error = BookError.AuthorNotFound
                };
            }

            // Find or create genres by name
            var genres = new List<Genre>();
            foreach (var genreName in bookDTO.Genres!)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName.ToString());
                if (genre == null)
                {
                    genre = new Genre { Name = genreName.ToString() };
                    await _context.Genres.AddAsync(genre);
                }
                genres.Add(genre);
            }
            await _context.SaveChangesAsync();  // Save new genres if any were created

            Book book = new Book
            {
                Title = bookDTO.Title!,
                Description = bookDTO.Description,
                Isbn = bookDTO.Isbn!,
                Language = bookDTO.Language!,
                Publisher = bookDTO.Publisher!,
                Authors = foundAuthors,
                Genres = genres,
                PublicationDate = bookDTO.PublicationDate,
                TotalPages = bookDTO.TotalPages
            };

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return new ServiceResponse<BookDTOOut, BookError>
            {
                Data = ToDTO(book),
                Error = BookError.None
            };
        }

        public async Task<BookDTOOut?> GetBook(int id)
        {
            Book? book = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsRemoved);
            return book == null ? null : ToDTO(book);
        }

        public async Task<IEnumerable<BookDTOOut>> GetAllBooks()
        {
            return await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Where(b => !b.IsRemoved)
                .Select(book => ToDTO(book))
                .ToListAsync();
        }

        public async Task<ServiceResponse<BookDTOOut, BookError>> UpdateBook(int id, BookDTOIn bookDTO)
        {
            Book? book = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return new ServiceResponse<BookDTOOut, BookError>
                {
                    Data = null,
                    Error = BookError.BookNotFound
                };
            }

            // Find or create genres by name
            var genres = new List<Genre>();
            foreach (var genreName in bookDTO.Genres!)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName.ToString());
                if (genre == null)
                {
                    genre = new Genre { Name = genreName.ToString() };
                    await _context.Genres.AddAsync(genre);
                }
                genres.Add(genre);
            }
            await _context.SaveChangesAsync();  // Save new genres if any were created

            book.Title = bookDTO.Title!;
            book.Description = bookDTO.Description;
            book.Authors = _context.Authors.Where(a => bookDTO.Authors!.Contains(a.Id)).ToList();
            book.Genres = genres;
            book.PublicationDate = bookDTO.PublicationDate;
            book.TotalPages = bookDTO.TotalPages;

            await _context.SaveChangesAsync();

            return new ServiceResponse<BookDTOOut, BookError>
            {
                Data = ToDTO(book),
                Error = BookError.None
            };
        }

        public async Task<bool> DeleteBook(int id)
        {
            Book? book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            // soft delete to maintain data integrity like loans and copy count
            // like for example imagine that the book is removed from the library and not deleted from the database
            book.IsRemoved = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreBook(int id)
        {
            Book? book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            book.IsRemoved = false;
            await _context.SaveChangesAsync();
            return true;
        }

        private static BookDTOOut ToDTO(Book book)
        {
            return new BookDTOOut(
                book.Id,
                book.Title,
                book.Description,
                book.Authors.Select(a => a.FirstName + " " + a.LastName).ToArray(),
                book.Genres.Select(g => g.Name).ToArray(),
                book.PublicationDate,
                book.TotalPages
            );
        }


    }

}
