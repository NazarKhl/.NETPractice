using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.DTOs;
using ReactApp1.Server.Interface;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbsenceController : ControllerBase
    {
        private readonly IAbsenceService _absenceService;

        public AbsenceController(IAbsenceService absenceService)
        {
            _absenceService = absenceService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var absence = _absenceService.GetAbsence(id);
            if (absence == null) return NotFound();
            return Ok(absence);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AbsenceDTO absenceDTO)
        {
            if (absenceDTO == null || absenceDTO.UserId <= 0 || !Enum.TryParse<AbsenceDTO.AbsenceType>(absenceDTO.Type.ToString(), out var type))
            {
                return BadRequest("Invalid absence data.");
            }

            absenceDTO.Type = type;
            _absenceService.AddAbsence(absenceDTO.UserId, absenceDTO);
            return CreatedAtAction(nameof(Get), new { id = absenceDTO.Id }, absenceDTO);
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _absenceService.DeleteAbsence(id);
            return NoContent();
        }
    }
}
