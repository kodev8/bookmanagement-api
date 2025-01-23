using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private AuthorService _authorService;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTOOut>>> GetAll()
        {
            return Ok(await _authorService.GetAllAuthors());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTOOut>> Get(int id)
        {
            AuthorDTOOut? author = await _authorService.GetAuthor(id);
            return author == null ? NotFound() : Ok(author);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<AuthorDTOOut>> Post([FromBody] AuthorDTOIn authorDTO)
        {
            AuthorDTOOut author = await _authorService.AddAuthor(authorDTO);
            return CreatedAtAction(nameof(Get), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<AuthorDTOOut>> Put(int id, [FromBody] AuthorDTOIn authorDTO)
        {
            AuthorDTOOut? author = await _authorService.UpdateAuthor(id, authorDTO);
            return author == null ? NotFound() : Ok(author);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _authorService.DeleteAuthor(id)) return NotFound();
            return NoContent();
        }
    }
}
