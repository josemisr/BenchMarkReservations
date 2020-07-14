namespace BenchmarkProjec.Models.Dto
{
    public class ServiceEmployeeDto
    {
        public int Id { get; set; }
        public int IdService { get; set; }
        public int IdEmployee { get; set; }

        public virtual EmployeeDto IdEmployeeNavigation { get; set; }
        public virtual ServiceDto IdServiceNavigation { get; set; }
    }
}
