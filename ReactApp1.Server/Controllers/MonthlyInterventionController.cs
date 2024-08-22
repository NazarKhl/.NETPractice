using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Interface;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthlyInterventionController : ControllerBase
    {
        private readonly IReadRepository<MonthlyInterventionModel> _readRepository;

        public MonthlyInterventionController(IReadRepository<MonthlyInterventionModel> readRepository)
        {
            _readRepository = readRepository;
        }

        [HttpGet("GetAllView")]
        public IActionResult GetAll()
        {
            var interventions = _readRepository.GetAll().ToList();
            return Ok(interventions);
        }
        [HttpGet("GetViewById{id}")]
        public IActionResult Get(int id)
        {
            var interventions = _readRepository.GetById(id);
            if (interventions == null) return NotFound();
            return Ok(interventions);
        }
    }
}
