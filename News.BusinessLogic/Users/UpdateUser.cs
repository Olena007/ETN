using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Interfaces;
using News.Entities;
using News.Enums;

namespace News.BusinessLogic.Users;

public class UpdateUser
{
    public class UpdateUserCommand : IRequest, IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;
    }


    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly INewsDbContext _context;

        public UpdateUserCommandHandler(INewsDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (entity == null)
                throw new NotFoundException(nameof(User), request.Id);

            entity.UserName = request.UserName;
            entity.Role = request.Role;
            entity.Email = request.Email;
            entity.PasswordHash = request.PasswordHash;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}