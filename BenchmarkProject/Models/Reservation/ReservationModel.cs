
using DataAccess.Models;
using System.Collections.Generic;

namespace BenchmarkProjec.Models.Reservation
{
    public class ReservationModel
    {
        public Reservations Reservation { get; set; }
        public IEnumerable<Reservations> Reservations { get; set; }
    }
}
