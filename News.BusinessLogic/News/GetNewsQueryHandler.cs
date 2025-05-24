using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Mappings;
using News.BusinessLogic.Common.Objects;
using News.BusinessLogic.Interfaces;
using News.Entities;

namespace News.BusinessLogic.News;

public class GetNewsQueryHandler(INewsDbContext context, IMapper mapper) : IRequestHandler<GetNewsQuery, NewsVm>
{
    public async Task<NewsVm> Handle(GetNewsQuery? request, CancellationToken cancellationToken)
    {
        var entities = context.News
            .Include(n => n.Authors)
            .Include(n => n.Categories)
            .Include(n => n.Videos)
            .Include(n => n.Source)
            .ThenInclude(s => s.Location)
            .Include(n => n.Location)
            .ThenInclude(l => l.Country)
            .Include(n => n.Views)
            .AsQueryable();
        ;

        if (request != null)
            entities = entities!
                .Skip((request.Pagging.Page - 1) * request.Pagging.Count)
                .Take(request.Pagging.Count);


        var entitiesList = await entities.ToListAsync(cancellationToken);
        var vms = mapper.Map<List<NewsModel>>(entitiesList);

        return new NewsVm { News = vms };
    }
}

public class NewsVm
{
    public IList<NewsModel> News { get; set; } = new List<NewsModel>();
}

public class GetNewsQuery : IRequest<NewsVm>
{
    public Pagging Pagging { get; set; } = new()
    {
        Count = 12,
        Page = 1
    };
}

public class NewsModel : IMapWith<Entities.News>
{
    public Guid NewsId { get; set; }
    public string Uri { get; set; }
    public string Lang { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string DateTime { get; set; }
    public double Sim { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public List<string> Links { get; set; }
    public string? Image { get; set; }
    public string? EventUri { get; set; }
    public List<AuthorModel> Authors { get; set; }
    public List<CategoryModel> Categories { get; set; }
    public SourceModel Source { get; set; }
    public List<VideoModel> Videos { get; set; }
    public LocationModel Location { get; set; }
    public List<ViewModel> Views { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entities.News, NewsModel>()
            .ForMember(dest => dest.NewsId, opt => opt.MapFrom(src => src.NewsId))
            .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.Authors))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
            .ForMember(dest => dest.Source,
                opt => opt.MapFrom(src => src.Source.FirstOrDefault())) // если Source - коллекция
            .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.Videos))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.FirstOrDefault()))
            .ForMember(dest => dest.Views, opt => opt.MapFrom(src => src.Views));
    }
}

public class SourceModel : IMapWith<Source>
{
    public Guid SourceId { get; set; }
    public Guid NewsId { get; set; }
    public string Uri { get; set; }
    public string? DataType { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public LocationModel? Location { get; set; }
    public bool? LocationValidated { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Source, SourceModel>();
    }
}

public class LocationModel : IMapWith<Location>
{
    public Guid LocationId { get; set; }
    public Guid NewsId { get; set; }
    public string? Type { get; set; }
    public List<CountryModel> Country { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Location, LocationModel>();
    }
}

public class CountryModel : IMapWith<Country>
{
    public Guid CountryId { get; set; }
    public Guid LocationId { get; set; }
    public string? Type { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Country, CountryModel>();
    }
}

public class CategoryModel : IMapWith<Category>
{
    public Guid CategoryId { get; set; }
    public Guid NewsId { get; set; }
    public string Uri { get; set; }
    public string? Label { get; set; }
    public int? Wgt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryModel>();
    }
}

public class AuthorModel : IMapWith<Author>
{
    public Guid AuthorId { get; set; }
    public Guid NewsId { get; set; }
    public string Uri { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public bool? IsAgency { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Author, AuthorModel>();
    }
}

public class VideoModel : IMapWith<Video>
{
    public Guid VideoId { get; set; }
    public Guid NewsId { get; set; }
    public string Uri { get; set; }
    public string? Label { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Video, VideoModel>();
    }
}

public class ViewModel : IMapWith<Entities.View>
{
    public Guid? ViewId { get; set; }
    public DateTime? ViewAt { get; set; }
    public Guid? UserId { get; set; }
    public string Uri { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entities.View, ViewModel>();
    }
}