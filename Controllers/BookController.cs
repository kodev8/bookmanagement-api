using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Errors;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDTOOut>>> GetAll()
        {
            return Ok(await _bookService.GetAllBooks());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BookDTOOut>> Get(int id)
        {
            BookDTOOut? book = await _bookService.GetBook(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<BookDTOOut>> Post([FromBody] BookDTOIn bookDTO)
        {
            var response = await _bookService.AddBook(bookDTO);
            if (response.Error != BookError.None)
                return BadRequest(new { error = response.Error, message = response.Error.GetMessage() });

            return CreatedAtAction(nameof(Get), new { id = response.Data!.Id }, response.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<BookDTOOut>> Put(int id, [FromBody] BookDTOIn bookDTO)
        {
            var response = await _bookService.UpdateBook(id, bookDTO);
            if (response.Error != BookError.None)
                return BadRequest(new { error = response.Error, message = response.Error.GetMessage() });

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _bookService.DeleteBook(id)) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Restore(int id)
        {
            if (!await _bookService.RestoreBook(id)) return NotFound();
            return Ok();
        }
    }
}
