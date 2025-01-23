using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Errors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Require authentication for all routes
    public class UsersController : ControllerBase
    {
        private readonly UserDataService _userDataService;

        public UsersController(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        [Authorize(Roles = "Admin")]  // Only admin can list all users
        public async Task<ActionResult<IEnumerable<UserDTOOut>>> Get()
        {
            return Ok(await _userDataService.GetUsers());
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]  // Only admin can get user details
        public async Task<ActionResult<UserDTOOut>> Get(string id)
        {
            UserDTOOut? user = await _userDataService.GetUser(id);
            return user == null ? NotFound() : Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        [AllowAnonymous]  // Allow registration
        public async Task<IActionResult> Post([FromBody] UserDTOIn user)
        {


            // check if user is authorized
            if (User.Identity?.Name != null && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            if (User.Identity?.Name == null || !User.IsInRole("Admin"))
            {
                user.Role = ERole.Member; // force member if admin is not trying to create the user
            }

            string? newUserId = await _userDataService.CreateUser(user);

            if (newUserId == null)
            {
                return BadRequest(new { success = false, message = "User creation failed" });
            }


            return CreatedAtAction(nameof(Get), new { id = newUserId }, new { success = true, message = "User created successfully", userId = newUserId });
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]  // Only admin can update users
        public async Task<IActionResult> Put(string id, [FromBody] UserDTOIn value)
        {
            UserDTOOut? userData = await _userDataService.UpdateUser(id, value);
            return userData == null ? NotFound() : Ok(new { success = true, message = "User updated", user = userData });
        }

        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]  // Only admin can update user roles
        public async Task<IActionResult> PutRole(string id, [FromBody] string role)
        {
            bool isUpdated = await _userDataService.UpdateRole(id, role);
            return isUpdated ? Ok(new { success = true, message = "User role updated" }) : NotFound();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]  // Only admin can delete users
        public async Task<IActionResult> Delete(string id)
        {
            bool isDeleted = await _userDataService.DeleteUser(id);
            return isDeleted ? Ok(new { success = true, message = "User deleted successfully" }) : NotFound();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO login)
        {
            var response = await _userDataService.Login(login);
            if (response.Error != LoginError.None)
                return BadRequest(new { error = response.Error, message = response.Error.GetMessage() });

            return Ok(response.Data);
        }
    }
}
