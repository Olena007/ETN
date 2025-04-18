using AutoMapper;
using News.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Bookings
{
    public class GetBooking
    {
        public class BookingVm : IMapWith<Booking>
        {
            public Guid? BookingId { get; set; }
            public DateTime? StartBooking { get; set; }
            public DateTime? EndBooking { get; set; }
            public string? Area { get; set; }

            public void Mapping(Profile profile)
            {
                profile.CreateMap<Booking, BookingVm>();
            }
        }

        public class GetBookingQuery : IRequest<BookingVm>
        {
            public Guid BookingId { get; set; }
        }
        public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingVm>
        {

            private readonly INewsDbContext _context;
            private readonly IMapper _mapper;

            public GetBookingQueryHandler(INewsDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BookingVm> Handle(GetBookingQuery request, CancellationToken cancellationToken)
            {
                var entity = await _context.Bookings
                    .Include(x => x.Cars)
                    .Include(x => x.Users)
                    .FirstOrDefaultAsync(x => x.BookingId == request.BookingId);

                if (entity == null)
                    throw new NotFoundException(nameof(Booking), request.BookingId);

                return _mapper.Map<BookingVm>(entity);
            }
        }
    }
}
