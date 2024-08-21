using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.DTOs;
using ReactApp1.Server.Interface;
using ReactApp1.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IReadRepository<User> _userRepository;

        public UserController(IUserService userService, IReadRepository<User> userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        [HttpGet("{id:int}/read")]
        public async Task<ActionResult<User>> GetUserReadOnly(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("read-all")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersReadOnly()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public IActionResult Get(int id)
        {
            var user = _userService.Get(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDTO userDTO)
        {
            _userService.Add(userDTO);
            return CreatedAtAction(nameof(Get), new { id = userDTO.Id }, userDTO);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserUpdateDTO userDTO)
        {
            _userService.Update(userDTO);
            return NoContent();
        }

        [HttpGet("active")]
        public IActionResult GetActiveUsers()
        {
            var users = _userService.GetActiveUsers();
            return Ok(users);
        }

        [HttpGet("inactive")]
        public IActionResult GetInactiveUsers()
        {
            var users = _userService.GetInactiveUsers();
            return Ok(users);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] UserPaginationDTO paginationDTO)
        {
            var (users, totalCount) = await _userService.GetPage(paginationDTO);
            return Ok(new { users, totalCount });
        }
    }
}
