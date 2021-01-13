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
    [Route("api/reportingstructure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeReportingStructure(String employeeId)
        {
            _logger.LogDebug($"Received reporting structure get request for '{employeeId}'");

            var reportingStructure = _reportingStructureService.Get(employeeId);

            if (reportingStructure == null)
            {
                return NotFound();
            }

            return Ok(reportingStructure);
        }
    }
}
