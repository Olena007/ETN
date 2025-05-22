using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Common.Objects;
using News.BusinessLogic.Interfaces;
using News.Models;


namespace News.BusinessLogic.News;

public class GetNewsQueryHandler(INewsDbContext context, IMapper mapper) : IRequestHandler<GetNewsQuery, NewsVm>
{
    public async Task<NewsVm> Handle(GetNewsQuery? request, CancellationToken cancellationToken)
    {
        var entities = context.News as IQueryable<Entities.News>;

        if (request != null)
            entities = entities!
                .Skip((request.Pagging.Page - 1) * request.Pagging.Count)
                .Take(request.Pagging.Count);


        var entitiesList = await entities.ToListAsync(cancellationToken);
        var vms = mapper.Map<List<NewsLookupDto>>(entitiesList);

        return new NewsVm { News = vms };
    }
}

public class NewsVm
{
    public IList<NewsLookupDto> News { get; set; } = null!;
}

public class NewsLookupDto : IMapWith<Entities.News>
{
    public string Lang { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string DateTime { get; set; }
    public double Sim { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public Source Source { get; set; }
    public List<Author> Authors { get; set; }
    public List<Concept> Concepts { get; set; }
    public List<Category> Categories { get; set; }
    public List<string> Links { get; set; }
    public List<Video> Videos { get; set; }
    public string Image { get; set; }
    public string EventUri { get; set; }
    public Location Location { get; set; }
    public Shares Shares { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entities.Location, Location>(); 
        profile.CreateMap<Entities.Source, Source>(); 
        profile.CreateMap<Entities.Author, Author>(); 
        profile.CreateMap<Entities.Category, Category>();
        profile.CreateMap<Entities.Video, Video>(); 
        profile.CreateMap<Entities.Shares, Shares>(); 
        profile.CreateMap<Entities.Ranking, Ranking>();
        profile.CreateMap<Entities.News, NewsLookupDto>();
    }
}

public class GetNewsQuery : IRequest<NewsVm>
{
    public Pagging Pagging { get; set; } = new()
    {
        Count = 12,
        Page = 1
    };
}