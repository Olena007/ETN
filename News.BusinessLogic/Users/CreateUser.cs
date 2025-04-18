using BCrypt.Net;
using News.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Users
{
    public class CreateUser
    {
        public class CreateUserCommand : IRequest<Guid>
        {
            public string? UserName { get; set; }
            public string? UserSurname { get; set; }
            public string? UserRole { get; set; }
            public string? UserEmail { get; set; } = null!;
            public string? Password { get; set; } = null!;
            public int? Level { get; set; }
        }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
        {
            private readonly INewsDbContext _context;
            public CreateUserCommandHandler(INewsDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var entity = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = request.UserName,
                    UserSurname = request.UserSurname,
                    UserRole = request.UserRole,
                    UserEmail = request.UserEmail,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Level = request.Level
                };

                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return (Guid)entity.UserId;
            }
        }
    }
}
