using MediatR;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.View;

public class CreateViewCommandHandler(INewsDbContext context) : IRequestHandler<CreateViewCommand, Guid>
{
    public async Task<Guid> Handle(CreateViewCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entities.View
        {
            ViewId = Guid.NewGuid(),
            ViewAt = request.ViewAt,
            UserId = request.UserId,
            Uri = request.Uri
        };

        await context.Views.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return (Guid)entity.ViewId;
    }
}

public class CreateViewCommand : IRequest<Guid>
{
    public DateTime? ViewAt { get; set; }
    public Guid? UserId { get; set; }
    public string Uri { get; set; }
}