using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.DTOs;
using ReactApp1.Server.Interface;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, IReadRepository<MonthlyInterventionModel> readRepository)
        {
            _userService = userService; 
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
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
        public IActionResult Put([FromBody] UserUpdateDTO userDTO)
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