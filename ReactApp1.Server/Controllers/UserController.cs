using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services;
using System.Collections.Generic;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpGet("active")]
        public ActionResult<IEnumerable<User>> GetActiveUsers()
        {
            var activeUsers = _userService.GetActiveUsers();
            return Ok(activeUsers);
        }

        [HttpGet("inactive")]
        public ActionResult<IEnumerable<User>> GetInactiveUsers()
        {
            var inactiveUsers = _userService.GetInactiveUsers();
            return Ok(inactiveUsers);
        }

        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            _userService.Add(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, User updatedUser)
        {
            var existingUser = _userService.Get(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            else
            {
                _userService.Update(updatedUser);
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                _userService.Delete(id);
                return NoContent();
            }
        }
    }
}
