using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Entities
{
    public class Car
    {
        public Guid? CarId { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? LicensePlate { get; set; }
        public int? YearOfIssue { get; set; }
        public bool? IsAvailable { get; set; }
        public string? Image { get; set; }
        public int? Level { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
