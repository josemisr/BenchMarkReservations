using BenchmarkProjec.Models.Dto;
using System.Collections.Generic;

namespace BenchmarkProjec.Models.ServiceEmployeeModel
{
    public class ServiceEmployeeModelDto
    {
        public ServiceEmployeeDto ServiceEmployee { get; set; }
        public IEnumerable<ServiceEmployeeDto> ServicesEmployeesList { get; set; }

    }
}
