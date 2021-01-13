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
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            Employee employee = _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
            
            if (employee != null)
            {
                //Only get direct report employeeIds, otherwise it turns into reporting structure.
                List<Employee> directReports = _employeeContext.Employees.Where(e => e.EmployeeId.Equals(id)).SelectMany(dr => dr.DirectReports).Select(dr => new Employee() { EmployeeId = dr.EmployeeId }).ToList();
                employee.DirectReports = directReports.Count == 0 ? null : directReports;
            }
            
            return employee;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
