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
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Compensation Add(String employeeId, double salary, DateTime effectiveDate)
        {
            Compensation compensation = new Compensation();
            Employee employee = _employeeContext.Employees.Where(e => e.EmployeeId.Equals(employeeId)).FirstOrDefault();

            if (employee == null)
            {
                throw new Exception("Must be a valid employee.");
            }
            else if (GetByEmployeeId(employeeId) != null)
            {
                throw new Exception("Employee already has a compensation.");
            }

            compensation.CompensationId = Guid.NewGuid().ToString();
            compensation.EmployeeId = employee.EmployeeId;
            compensation.Salary = salary;
            compensation.EffectiveDate = effectiveDate;
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Object GetByEmployeeId(string employeeId)
        {

            //Only get direct report employeeIds, otherwise it turns into reporting structure.
            List<Employee> directReports = _employeeContext.Employees.Where(e => e.EmployeeId.Equals(employeeId)).SelectMany(dr => dr.DirectReports).Select(dr => new Employee() { EmployeeId = dr.EmployeeId }).ToList();
            return _employeeContext.Compensations.Where(c => c.EmployeeId.Equals(employeeId))
                .Join(
                    _employeeContext.Employees,
                    employee => employee.EmployeeId,
                    compensation => compensation.EmployeeId,
                    (compensation, employee) => new 
                    { 
                        Employee = new Employee() 
                        {
                            EmployeeId = employee.EmployeeId,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Position = employee.Position,
                            Department = employee.Department,
                            DirectReports = directReports.Count == 0 ? null : directReports
                        },
                        compensation.Salary,
                        EffectiveDate = compensation.EffectiveDate.ToShortDateString()
                    }
                ).FirstOrDefault();
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
