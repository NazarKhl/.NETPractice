using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Interface;
using ReactApp1.Server.Models;
using System;
using System.Linq;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcedureInterventionController : ControllerBase
    {
        private readonly IProcedureInterventionRepository<ProcedureInterventionModel> _repository;

        public ProcedureInterventionController(IProcedureInterventionRepository<ProcedureInterventionModel> repository)
        {
            _repository = repository;
        }

        [HttpGet("GetProcedureById/{id}")]
        public IActionResult Get(int id)
        {
            var intervention = _repository.GetById(id);
            if (intervention == null) return NotFound();
            return Ok(intervention);
        }

        [HttpGet("GetAllProcedure")]
        public IActionResult GetAll([FromQuery] int customerId, [FromQuery] string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
            {
                return BadRequest("Invalid date format");
            }

            var interventions = _repository.GetAll(customerId, parsedDate).ToList();
            return Ok(interventions);
        }
    }
}
