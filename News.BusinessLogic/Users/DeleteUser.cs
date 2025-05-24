using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Interfaces;
using News.Entities;

namespace News.BusinessLogic.Users;

public class DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly INewsDbContext _context;

        public DeleteUserCommandHandler(INewsDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == request.Id);

            if (entity == null)
                throw new NotFoundException(nameof(User), request.Id);

            _context.Users.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}