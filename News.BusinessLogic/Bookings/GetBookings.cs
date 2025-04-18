using AutoMapper.QueryableExtensions;
using AutoMapper;
using News.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Common.Objects;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Bookings
{
    public class GetBookings
    {
        public class BookingsVm
        {
            public IList<BookingLookupDto> Bookings { get; set; }
        }
        public class BookingLookupDto : IMapWith<Booking>
        {
            public Guid? UserId { get; set; }
            public Guid? CarId { get; set; }
            public DateTime? StartBooking { get; set; }
            public DateTime? EndBooking { get; set; }
            public string? Area { get; set; }

            public void Mapping(Profile profile)
            {
                profile.CreateMap<Booking, BookingLookupDto>();
            }
        }
        public class GetBookingsQuery : IRequest<BookingsVm>
        {
            public Pagging Pagging { get; set; } = new Pagging
            {
                Count = 12,
                Page = 1
            };
        }
        public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, BookingsVm>
        {

            private readonly INewsDbContext _context;
            private readonly IMapper _mapper;

            public GetBookingsQueryHandler(INewsDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BookingsVm> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
            {
                var entities = _context.Bookings as IQueryable<Booking>;

                if (request != null)
                {
                    entities = entities
                           .Skip((request.Pagging.Page - 1) * request.Pagging.Count)
                           .Take(request.Pagging.Count);
                }


                var vms = await entities
                    .ProjectTo<BookingLookupDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new BookingsVm { Bookings = vms };
            }
        }
    }
}
