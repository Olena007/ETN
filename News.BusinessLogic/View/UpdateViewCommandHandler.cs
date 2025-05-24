using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.View;

public class UpdateViewCommandHandler(INewsDbContext context) : IRequestHandler<UpdateViewCommand>
{
    public async Task<Unit> Handle(UpdateViewCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Views
            .FirstOrDefaultAsync(x => x.ViewId == request.ViewId, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(View), request.ViewId!);

        entity.UserId = request.UserId;
        entity.ViewAt = request.ViewAt;
        entity.UserId = request.UserId;
        entity.Uri = request.Uri;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class UpdateViewCommand : IRequest
{
    public Guid? ViewId { get; set; }
    public DateTime? ViewAt { get; set; }
    public Guid? UserId { get; set; }
    public string Uri { get; set; }
}