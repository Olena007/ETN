using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using News.BusinessLogic.Interfaces;
using News.Entities;
using News.Enums;

namespace News.BusinessLogic.Users;

public class CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;
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
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Role = request.Role,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash),
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}