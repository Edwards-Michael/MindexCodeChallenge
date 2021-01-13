using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class ReportingStructureRepository : IReportingStructureRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IReportingStructureRepository> _logger;

        public ReportingStructureRepository(ILogger<IReportingStructureRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public ReportingStructure Get(String employeeId)
        {
            Employee employee = _employeeContext.Employees.Where(e => e.EmployeeId.Equals(employeeId)).FirstOrDefault();
            ReportingStructure reportingStructure = new ReportingStructure() { Employee = employee};
            if (employee == null)
            {
                return null;
            }
            else
            {
                return GetEmployeeReportingStructure(employee.EmployeeId, new List<String>(), reportingStructure, employee);
            }
            
        }

        private ReportingStructure GetEmployeeReportingStructure(String employeeId, List<String> reportingEmployees, ReportingStructure reportingStructure, Employee employee)
        {

            List<Employee> directReports = _employeeContext.Employees.Where(e => e.EmployeeId.Equals(employeeId))
                .Select(e => e.DirectReports)
                .SelectMany(directReport => directReport)
                // Ensure the reporting employee is not the top level employee and is not a duplicate to prevent an infinte stack call.
                .Where(re => !re.EmployeeId.Equals(reportingStructure.Employee.EmployeeId) && !reportingEmployees.Contains(re.EmployeeId)).ToList();
            
            if (employee != null)
            {
                employee.DirectReports = directReports.Count == 0 ? null : directReports;
            }
             
            foreach(Employee directReport in directReports)
            {
                reportingEmployees.Add(directReport.EmployeeId);
                GetEmployeeReportingStructure(directReport.EmployeeId, reportingEmployees, reportingStructure, employee.DirectReports.Where(e => e.EmployeeId.Equals(directReport.EmployeeId)).FirstOrDefault());
            }
            reportingStructure.Employee = employee;
            reportingStructure.NumberOfReports = reportingEmployees.Count();
            // Return the generated employee reporting structure with the count of reporting employees.
            return reportingStructure;
        }
    }
}
