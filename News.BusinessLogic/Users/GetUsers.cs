using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Common.Objects;
using News.BusinessLogic.Interfaces;
using News.Entities;

namespace News.BusinessLogic.Users;

public class GetUsers
{
    public class UsersVm
    {
        public IList<UserLookupDto> Users { get; set; } = null!;
    }

    public class UserLookupDto : IMapWith<User>
    {
        public string? UserName { get; set; }
        public string? UserSurname { get; set; }
        public string? UserRole { get; set; }
        public string? UserEmail { get; set; }
        public string? Password { get; set; }
        public int? Level { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserLookupDto>();
        }
    }

    public class GetUsersQuery : IRequest<UsersVm>
    {
        public Pagging Pagging { get; set; } = new()
        {
            Count = 12,
            Page = 1
        };
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, UsersVm>
    {
        private readonly INewsDbContext _context;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(INewsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsersVm> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var entities = _context.Users as IQueryable<User>;

            if (request != null)
                entities = entities
                    .Skip((request.Pagging.Page - 1) * request.Pagging.Count)
                    .Take(request.Pagging.Count);


            var vms = await entities
                .ProjectTo<UserLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new UsersVm { Users = vms };
        }
    }
}