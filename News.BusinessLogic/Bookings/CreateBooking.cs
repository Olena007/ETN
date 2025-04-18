using News.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.BusinessLogic.Interfaces;
using static News.BusinessLogic.Users.CreateUser;

namespace News.BusinessLogic.Bookings
{
    public class CreateBooking
    {
        public class CreateBookingCommand : IRequest<Guid>
        {
            public Guid? UserId { get; set; }
            public Guid? CarId { get; set; }
            public DateTime? StartBooking { get; set; }
            public DateTime? EndBooking { get; set; }
            public string? Area { get; set; }
        }
        public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
        {
            private readonly INewsDbContext _context;
            public CreateBookingCommandHandler(INewsDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
            {
                var entity = new Booking
                {
                    BookingId = Guid.NewGuid(),
                    UserId = request.UserId,
                    CarId = request.CarId,
                    StartBooking = request.StartBooking,
                    EndBooking = request.EndBooking,
                    Area = request.Area
                };

                await _context.Bookings.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return (Guid)entity.BookingId;
            }
        }
    }
}
