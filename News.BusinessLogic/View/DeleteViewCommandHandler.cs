using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.View;

public class DeleteViewCommandHandler(INewsDbContext context) : IRequestHandler<DeleteViewCommand>
{
    public async Task<Unit> Handle(DeleteViewCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Views
            .FirstOrDefaultAsync(x => x.ViewId == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(View), request.Id);

        context.Views.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class DeleteViewCommand : IRequest
{
    public Guid Id { get; set; }
}