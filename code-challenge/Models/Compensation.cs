using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        // Compensation needs an Id for the context database
        public String CompensationId { get; set; }
        // Only EmployeeId is needed and more information should not be stored in the database. Additional employee data can be retrieved by joining.
        public String EmployeeId { get; set; }
        public double Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
