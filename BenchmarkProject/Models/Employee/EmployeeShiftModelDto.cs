using BenchmarkProjec.Models.Dto;
using System;
using System.Collections.Generic;

namespace BenchmarkProjec.Models.EmployeeShiftModel
{
    public class EmployeeShiftModelDto
    {
        public EmployeeShiftDto EmployeeShift { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<EmployeeShiftDto> EmployeeShiftsList { get; set; }


    }
}
