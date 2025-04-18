using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.Entities;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Common.Objects;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Cars
{
    public class GetCars
    {
        public class CarsVm
        {
            public IList<CarLookupDto> Cars { get; set; }
        }
        public class CarLookupDto : IMapWith<Car>
        {
            public string? Brand { get; set; }
            public string? Model { get; set; }
            public string? LicensePlate { get; set; }
            public int? YearOfIssue { get; set; }
            public bool? IsAvailable { get; set; }
            public string? Image { get; set; }
            public int? Level { get; set; }
            public void Mapping(Profile profile)
            {
                profile.CreateMap<Car, CarLookupDto>();
            }
        }
        public class GetCarsQuery : IRequest<CarsVm>
        {
            public Pagging Pagging { get; set; } = new Pagging
            {
                Count = 12,
                Page = 1
            };
        }
        public class GetCarsQueryHandler : IRequestHandler<GetCarsQuery, CarsVm>
        {

            private readonly INewsDbContext _context;
            private readonly IMapper _mapper;

            public GetCarsQueryHandler(INewsDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CarsVm> Handle(GetCarsQuery request, CancellationToken cancellationToken)
            {
                var entities = _context.Cars as IQueryable<Car>;

                if (request != null)
                {
                    entities = entities
                           .Skip((request.Pagging.Page - 1) * request.Pagging.Count)
                           .Take(request.Pagging.Count);
                }


                var vms = await entities
                    .ProjectTo<CarLookupDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new CarsVm { Cars = vms };
            }
        }
    }
}
