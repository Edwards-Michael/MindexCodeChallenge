using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Object GetByEmployeeId(String employeeId);
        Compensation Add(String employeeId, double salary, DateTime effectiveDate);
        Task SaveAsync();
    }
}