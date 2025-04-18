using AutoMapper;
using News.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Users
{
    public class GetUser
    {
        public class UserVm : IMapWith<User>
        {
            public Guid? UserId { get; set; }
            public string? UserName { get; set; }
            public string? UserSurname { get; set; }
            public string? UserRole { get; set; }
            public string? UserEmail { get; set; } = null!;
            [JsonIgnore]
            public string? Password { get; set; } = null!;
            public int? Level { get; set; }
            public ICollection<Booking> Bookings { get; set; }

            public void Mapping(Profile profile)
            {
                profile.CreateMap<User, UserVm>();
            }
        }

        public class GetUserQuery : IRequest<UserVm>
        {
            public Guid UserId { get; set; }
        }
        public class GetUserQueryByEmail : IRequest<UserVm>
        {
            public string UserEmail { get; set; }
        }

        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserVm>
        {

            private readonly INewsDbContext _context;
            private readonly IMapper _mapper;

            public GetUserQueryHandler(INewsDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var entity = await _context.Users
                    .Include(x => x.Bookings)
                    .FirstOrDefaultAsync(x => x.UserId == request.UserId);

                if (entity == null)
                    throw new NotFoundException(nameof(User), request.UserId);

                return _mapper.Map<UserVm>(entity);
            }
        }
        public class GetUserQueryByEmailHandler : IRequestHandler<GetUserQueryByEmail, UserVm>
        {

            private readonly INewsDbContext _context;
            private readonly IMapper _mapper;

            public GetUserQueryByEmailHandler(INewsDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserVm> Handle(GetUserQueryByEmail request, CancellationToken cancellationToken)
            {
                var entity = await _context.Users
                    .Include(x => x.Bookings)
                    .FirstOrDefaultAsync(x => x.UserEmail == request.UserEmail);

                if (entity == null)
                    throw new NotFoundException(nameof(User), request.UserEmail);

                return _mapper.Map<UserVm>(entity);
            }
        }
    }
}
