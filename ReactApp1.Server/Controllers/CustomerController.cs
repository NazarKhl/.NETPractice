using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.DTOs;
using ReactApp1.Server.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _customerService.GetAll();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var customer = _customerService.Get(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }
    }
}
