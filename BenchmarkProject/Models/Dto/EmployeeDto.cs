using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenchmarkProjec.Models.Dto
{
    public class EmployeeDto
    {
        public EmployeeDto()
        {
            EmployeesShifts = new HashSet<EmployeeShiftDto>();
            Reservations = new HashSet<ReservationDto>();
            ServicesEmployees = new HashSet<ServiceEmployeeDto>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Surname2 { get; set; }
        [Required(ErrorMessage = "IdCard Required")]
        public string IdCard { get; set; }

        public virtual ICollection<EmployeeShiftDto> EmployeesShifts { get; set; }
        public virtual ICollection<ReservationDto> Reservations { get; set; }
        public virtual ICollection<ServiceEmployeeDto> ServicesEmployees { get; set; }
    }
}
