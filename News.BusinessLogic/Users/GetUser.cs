using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Interfaces;
using News.Entities;
using News.Enums;

namespace News.BusinessLogic.Users;

public class GetUser
{
    public class UserVm : IMapWith<User>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

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
        public string UserEmail { get; set; } = null!;
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
                .FirstOrDefaultAsync(x => x.Id == request.UserId);

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
                .FirstOrDefaultAsync(x => x.Email == request.UserEmail);

            if (entity == null)
                throw new NotFoundException(nameof(User), request.UserEmail);

            return _mapper.Map<UserVm>(entity);
        }
    }
}