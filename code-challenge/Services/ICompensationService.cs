using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Object GetByEmployeeId(String employeeId);
        Compensation Create(String employeeId, double salary, DateTime effectiveDate);
    }
}
