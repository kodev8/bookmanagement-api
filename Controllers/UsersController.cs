using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDataService _userDataService;

        public UsersController(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public ActionResult<IEnumerable<UserDTOOut>> Get()
        {
            return Ok(_userDataService.GetUsers());
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            UserDTOOut? user = _userDataService.GetUser(id);
            return user == null ? NotFound() : Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] UserDTOIn user)
        {

            string newUserId = _userDataService.CreateUser(user);
            
            return CreatedAtAction(
            nameof(Get), 
            new { id = newUserId }, 
            new { success = true, message = "User created successfully", userId = newUserId}
            );
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] UserDTOIn value)
        {
            UserDTOOut? userData = _userDataService.UpdateUser(id, value);

            return userData == null ? NotFound() : Ok(new { 
                success =  true,
                message = "User updated",
                user = userData
            });

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool isDeleted = _userDataService.DeleteUser(id);
            return isDeleted ? Ok(new { success = true, message = "User deleted successfully" }) : NotFound();
        }
    }
}
