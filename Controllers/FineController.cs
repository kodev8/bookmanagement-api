using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Errors;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Require authentication for all routes
    public class FineController : ControllerBase
    {
        private readonly FineService _fineService;

        public FineController(FineService fineService)
        {
            _fineService = fineService;
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<IEnumerable<FineDTOOut>>> GetAllFines()
        {
            return Ok(await _fineService.GetAllFines());
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,Staff")] // get all fines for a user
        public async Task<ActionResult<IEnumerable<FineDTOOut>>> GetAllUserFines(string userId)
        {
            return Ok(await _fineService.GetUserFines(userId));
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]

        public async Task<ActionResult<FineDTOOut>> Get(int id)
        {
            var fine = await _fineService.GetFine(id);
            return fine == null ? NotFound() : Ok(fine);
        }

        [HttpGet("member")]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult<IEnumerable<FineDTOOut>>> GetMemberFines()
        {
            return Ok(await _fineService.GetUserFines(User.Identity?.Name ?? string.Empty));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<FineDTOOut>> CreateFine([FromBody] FineDTOIn fineDTO)
        {
            var response = await _fineService.AddFine(fineDTO);
            if (response.Error != BookLoanError.None)
                return BadRequest(new { error = response.Error, message = response.Error.GetMessage() });

            return Ok(response.Data);
        }

        [HttpPatch("pay/{id}")]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult<FineDTOOut>> PayFine(int id)
        {
            var userId = User.Identity?.Name ?? string.Empty;
            var response = await _fineService.UpdateFineDetails(id, new FineDTOInPartial { UserId = userId, PaidDate = DateTime.Now, Status = EFineStatus.Paid });
            if (response.Error != BookLoanError.None)
                return BadRequest(new { error = response.Error, message = response.Error.GetMessage() });

            return Ok(response.Data);
        }
    }
}
