using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.EmployeeId}'");

            Compensation newCompensation = new Compensation();
            try
            {
               compensation = _compensationService.Create(compensation.EmployeeId, compensation.Salary, compensation.EffectiveDate);
            }
            catch(Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return CreatedAtRoute("getByEmployeeId", new { compensation.EmployeeId }, new { compensation.EmployeeId, compensation.Salary, EffectiveDate = compensation.EffectiveDate.ToShortDateString() });
        }

        [HttpGet("{employeeId}", Name = "getByEmployeeId")]
        public IActionResult GetByEmployeeId(String employeeId)
        {
            _logger.LogDebug($"Received compensation get request for '{employeeId}'");

            var compensation = _compensationService.GetByEmployeeId(employeeId);

            if (compensation == null)
            {
                return NotFound();
            }

            return Ok(compensation);
        }
    }
}
