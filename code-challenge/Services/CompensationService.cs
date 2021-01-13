using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public Compensation Create(String employeeId, double salary, DateTime effeectiveDate)
        {
            Compensation compensation = new Compensation();
            if(salary < 0)
            {
                throw new Exception("Salary must be greater than zero.");
            }
            else if(String.IsNullOrEmpty(employeeId))
            {
                throw new Exception("Must be a valid employee.");
            }
            else
            {
                compensation = _compensationRepository.Add(employeeId, salary, effeectiveDate);
                _compensationRepository.SaveAsync().Wait();
            }
            return compensation;
        }

        public Object GetByEmployeeId(String employeeId)
        {
            Compensation compensation = null;
            if(!String.IsNullOrEmpty(employeeId))
            {
                return _compensationRepository.GetByEmployeeId(employeeId);
            }

            return compensation;
        }
    }
}
