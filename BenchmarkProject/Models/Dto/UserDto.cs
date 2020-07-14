using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenchmarkProjec.Models.Dto
{
    public class UserDto
    {
        public UserDto()
        {
            Reservations = new HashSet<ReservationDto>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Surname2 { get; set; }
        [Required(ErrorMessage = "IdCard Required")]
        public string IdCard { get; set; }
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Birthday Required")]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "Role Required")]
        public int IdRole { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        public virtual RoleDto IdRoleNavigation { get; set; }
        public virtual ICollection<ReservationDto> Reservations { get; set; }
    }
}
