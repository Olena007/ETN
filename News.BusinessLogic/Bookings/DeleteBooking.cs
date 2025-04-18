using News.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Bookings
{
    public class DeleteBooking
    {
        public class DeleteBookingCommand : IRequest
        {
            public Guid Id { get; set; }
        }

        public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
        {

            private readonly INewsDbContext _context;

            public DeleteBookingCommandHandler(INewsDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Bookings
                    .FirstOrDefaultAsync(x => x.BookingId == request.Id);

                if (entity == null)
                    throw new NotFoundException(nameof(Booking), request.Id);

                _context.Bookings.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
