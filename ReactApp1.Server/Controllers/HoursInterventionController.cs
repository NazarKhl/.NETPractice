using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Interface;
using ReactApp1.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<HoursInterventionModel>>> GetHoursInterventions(int userId, DateTime yearMonth)
        {
            var interventions = _repository.GetAll(userId, yearMonth);
            return Ok(await interventions.ToListAsync());
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
