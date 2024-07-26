using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Core.Entities
{
    public class User 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NumberOfReservation { get; set; }
        public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
    }
}
