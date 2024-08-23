using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Interface;
using ReactApp1.Server.Models;
using System;
using System.Linq;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoursInterventionController : ControllerBase
    {
        private readonly IHoursInterventionRepository<HoursInterventionModel> _repository;

        public HoursInterventionController(IHoursInterventionRepository<HoursInterventionModel> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int? userId, [FromQuery] int? customerId, [FromQuery] string date)
        {
            DateTime? parsedDate = null;
            if (!string.IsNullOrEmpty(date))
            {
                if (!DateTime.TryParse(date, out DateTime tempDate))
                {
                    return BadRequest("Invalid date format");
                }
                parsedDate = tempDate;
            }

            var interventions = _repository.GetAll(userId, customerId, parsedDate).ToList();
            return Ok(interventions);
        }

        [HttpGet("{id}")]
        public ActionResult<HoursInterventionModel> GetHoursInterventionById(int id)
        {
            var intervention = _repository.GetById(id);
            if (intervention == null)
            {
                return NotFound();
            }
            return Ok(intervention);
        }
    }
}
