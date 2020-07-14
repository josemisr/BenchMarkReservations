using System;
using System.ComponentModel.DataAnnotations;

namespace BenchmarkProjec.Models.Dto
{
    public class ReservationDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Service Required")]
        public int IdService { get; set; }
        [Required(ErrorMessage = "Employee Required")]
        public int IdEmployee { get; set; }
        [Required(ErrorMessage = "User Required")]
        public int IdUser { get; set; }
        [Required(ErrorMessage = "Date Required")]
        public DateTime Date { get; set; }

        public EmployeeDto IdEmployeeNavigation { get; set; }
        public ServiceDto IdServiceNavigation { get; set; }
        public UserDto IdUserNavigation { get; set; }
    }
}
