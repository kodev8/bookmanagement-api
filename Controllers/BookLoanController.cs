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
    public class BookLoanController : ControllerBase
    {
        private readonly BookLoanService _bookLoanService;

        public BookLoanController(BookLoanService bookLoanService)
        {
            _bookLoanService = bookLoanService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<IEnumerable<BookLoanDTOOut>>> GetUserLoans()
        {
            return Ok(await _bookLoanService.GetAllLoans());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<BookLoanDTOOut>> Get(int id)
        {
            var loan = await _bookLoanService.GetBookLoan(id);
            return loan == null ? NotFound() : Ok(loan);
        }

        [HttpGet("member")]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult<IEnumerable<BookLoanDTOOut>>> GetMemberLoans()
        {
            return Ok(await _bookLoanService.GetUserLoans(User.Identity?.Name ?? string.Empty));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<BookLoanDTOOut>> CreateLoan([FromBody] BookLoanDTOIn loanDTO)
        {
            var response = await _bookLoanService.AddBookLoan(loanDTO);
            if (response.Error != BookLoanError.None)
                return BadRequest(new { error = response.Error, message = response.Error.GetMessage() });

            return CreatedAtAction(nameof(Get), new { id = response.Data!.Id }, response.Data);
        }

        [HttpPut("{id}/return")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<BookLoanDTOOut>> ReturnBook(int id, [FromBody] string userId)
        {
            var response = await _bookLoanService.ReturnBookLoan(id, userId);
            if (response.Error != BookLoanError.None)
                return BadRequest(new { error = response.Error, message = response.Error.GetMessage() });

            return Ok(response.Data);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<BookLoanDTOOut>> UpdateStatus(int id, [FromBody] EStatus status)
        {
            var response = await _bookLoanService.UpdateBookLoanStatus(id, status);
            if (response.Error != BookLoanError.None)
                return BadRequest(response.Error);

            return Ok(response.Data);
        }
    }
}
