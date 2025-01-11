using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserDataService userDataService;

        public UsersController(UserDataService userDataServiceArg)
        {
            userDataService = userDataServiceArg;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<UserDTOOut> Get()
        {
            return userDataService.GetUsers();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public UserDTOOut? Get(string id)
        {
            return userDataService.GetUser(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public UserDTOIn Post([FromBody] UserDTOIn user)
        {

            userDataService.CreateUser(user);

            return user;
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public UserDTOOut? Put(string id, [FromBody] UserDTOIn value)
        {
            return userDataService.UpdateUser(id, value);

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            userDataService.DeleteUser(id);
        }
    }
}
