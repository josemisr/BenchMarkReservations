
using BenchmarkProjec.Models.Dto;
using DataAccess.Models;
using System.Collections.Generic;

namespace BenchmarkProjec.Models.Reservation
{
    public class ReservationModelDto
    {
        public ReservationDto  Reservation { get; set; }
        public IEnumerable<ReservationDto> Reservations { get; set; }
    }
}
