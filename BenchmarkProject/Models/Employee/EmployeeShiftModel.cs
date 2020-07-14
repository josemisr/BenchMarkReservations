using DataAccess.Models;
using System;
using System.Collections.Generic;

namespace BenchmarkProjec.Models.EmployeeShiftModel
{
    public class EmployeeShiftModel
    {
        public EmployeesShifts EmployeeShift { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<EmployeesShifts> EmployeeShiftsList { get; set; }


    }
}
