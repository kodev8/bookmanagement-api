using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDTOOut>> GetAll()
        {
            return Ok(_bookService.GetAllBooks());
        }

        [HttpGet("{id}")]
        public ActionResult<BookDTOOut> Get(int id)
        {
            BookDTOOut? book = _bookService.GetBook(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public ActionResult<BookDTOOut> Post([FromBody] BookDTOIn bookDTO)
        {
            BookDTOOut book = _bookService.AddBook(bookDTO);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public ActionResult<BookDTOOut> Put(int id, [FromBody] BookDTOIn bookDTO)
        {
            BookDTOOut? book = _bookService.UpdateBook(id, bookDTO);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!_bookService.DeleteBook(id)) return NotFound();
            return NoContent();
        }
    }
}
