using BenchmarkProjec.Models.Dto;
using DataAccess.Models;
using System.Collections.Generic;

namespace BenchmarkProjec.Models.ServiceEmployeeModel
{
    public class ServiceEmployeeModel
    {
        public ServicesEmployees ServiceEmployee { get; set; }
        public IEnumerable<ServicesEmployees> ServicesEmployeesList { get; set; }

    }
}
