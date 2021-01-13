using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface IReportingStructureService
    {
        ReportingStructure Get(String employeeId);
    }
}
