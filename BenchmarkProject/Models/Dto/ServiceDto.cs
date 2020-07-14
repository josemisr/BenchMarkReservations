using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenchmarkProjec.Models.Dto
{
    public class ServiceDto
    {
        public ServiceDto()
        {
            Reservations = new HashSet<ReservationDto>();
            ServicesEmployees = new HashSet<ServiceEmployeeDto>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price Required")]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ReservationDto> Reservations { get; set; }
        public virtual ICollection<ServiceEmployeeDto> ServicesEmployees { get; set; }
    }
}
