using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Common.Objects;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.View;

public class GetViewsQueryHandler(INewsDbContext context, IMapper mapper) : IRequestHandler<GetViewsQuery, ViewsVm>
{
    public async Task<ViewsVm> Handle(GetViewsQuery? request, CancellationToken cancellationToken)
    {
        var entities = context.Views as IQueryable<Entities.View>;

        if (request != null)
            entities = entities
                .Skip((request.Pagging.Page - 1) * request.Pagging.Count)
                .Take(request.Pagging.Count);


        var vms = await entities
            .ProjectTo<ViewLookupDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new ViewsVm { Views = vms };
    }
}

public class ViewsVm
{
    public IList<ViewLookupDto> Views { get; set; } = null!;
}

public class ViewLookupDto : IMapWith<Entities.View>
{
    public DateTime? ViewAt { get; set; }
    public Guid? UserId { get; set; }
    public string Uri { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entities.View, ViewLookupDto>();
    }
}

public class GetViewsQuery : IRequest<ViewsVm>
{
    public Pagging Pagging { get; set; } = new()
    {
        Count = 12,
        Page = 1
    };
}