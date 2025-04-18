using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Entities
{
    public class Booking
    {
        public Guid? BookingId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CarId { get; set; }
        public DateTime? StartBooking { get; set; }
        public DateTime? EndBooking { get; set; }
        public string? Area { get; set; }    
        public Car Cars { get; set; }
        public User Users { get; set; }
    }
}
