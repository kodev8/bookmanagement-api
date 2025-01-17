using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class BookService
    {
        private List<Book> _books = new(); // since we have not set up db yet
        private int _nextId = 1;

        public BookDTOOut AddBook(BookDTOIn bookDTO)
        {
            Book book = new Book
            {
                Id = _nextId++,
                FullTitle = bookDTO.Title,
                Description = bookDTO.Description,
                Author = bookDTO.Author,
                Genres = bookDTO.Genres,
                PublicationDate = bookDTO.PublicationDate,
                NumberOfPages = bookDTO.NumberOfPages
            };

            _books.Add(book);
            return ToDTO(book);
        }

        public BookDTOOut? GetBook(int id)
        {
            Book? book = _books.FirstOrDefault(b => b.Id == id);
            return book == null ? null : ToDTO(book);
        }

        public IEnumerable<BookDTOOut> GetAllBooks()
        {
            return _books.Select(ToDTO);
        }

        public BookDTOOut? UpdateBook(int id, BookDTOIn bookDTO)
        {
            Book? book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return null;

            book.FullTitle = bookDTO.Title;
            book.Description = bookDTO.Description;
            book.Author = bookDTO.Author;
            book.Genres = bookDTO.Genres;
            book.PublicationDate = bookDTO.PublicationDate;
            book.NumberOfPages = bookDTO.NumberOfPages;

            return ToDTO(book);
        }

        public bool DeleteBook(int id)
        {
            Book? book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return false;

            return _books.Remove(book);
        }

        private static BookDTOOut ToDTO(Book book)
        {
            return new BookDTOOut(
                book.Id,
                book.FullTitle,
                book.Description,
                book.Author,
                book.Genres,
                book.PublicationDate,
                book.NumberOfPages
            );
        }
    }
}