using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IReportingStructureRepository _reportingStructureRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IReportingStructureRepository reportingStructureRepository)
        {
            _reportingStructureRepository = reportingStructureRepository;
            _logger = logger;
        }

        public ReportingStructure Get(String employeeId)
        {
            if(!String.IsNullOrEmpty(employeeId))
            {
                return _reportingStructureRepository.Get(employeeId);
            }

            return null;
        }
    }
}
