using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Interfaces;
using News.Entities;

namespace News.BusinessLogic.View;

public class GetViewQueryHandler(INewsDbContext context, IMapper mapper): IRequestHandler<GetViewQuery, ViewVm>
{
    public async Task<ViewVm> Handle(GetViewQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Views
            .Include(x => x.News)
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.ViewId == request.ViewId, cancellationToken: cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Booking), request.ViewId);

        return mapper.Map<ViewVm>(entity);
    }
}

public class ViewVm : IMapWith<Entities.View>
{
    public Guid? ViewId { get; set; }
    public DateTime? ViewAt { get; set; }
    public Guid? UserId { get; set; }
    public string Uri { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entities.View, ViewsVm>();
    }
}

public class GetViewQuery : IRequest<ViewsVm>, IRequest<ViewVm>
{
    public Guid ViewId { get; set; }
}