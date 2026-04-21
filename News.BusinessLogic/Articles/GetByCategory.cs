using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Articles;

public class GetByCategory
{
    public class GetByCategoryQuery : IRequest<List<ArticleByCategoryDto>>
    {
        public int Count { get; set; } = 1;
        public string Category { get; set; }
    }

    public class ArticleByCategoryDto
    {
        public Guid Id { get; set; }
        public string Uuid { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Author { get; set; }
        public DateTime Published { get; set; }
        public List<string> Categories { get; set; } = new();
        public GetArticle.ThreadInfoDto? Thread { get; set; }
    }

    public class GetByCategoryQueryHandler(INewsDbContext context)
        : IRequestHandler<GetByCategoryQuery, List<ArticleByCategoryDto>>
    {
        public async Task<List<ArticleByCategoryDto>> Handle(GetByCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var query = context.Articles
                .Include(a => a.Thread)
                .Include(a => a.Categories)
                .AsQueryable();

            return await query
                .OrderByDescending(a => a.Published)
                .Where(a => a.Categories.Any(c => c.Name == request.Category))
                .Where(x => x.Language == "english")
                .Take(request.Count)
                .Select(a => new ArticleByCategoryDto
                {
                    Id = a.Id,
                    Uuid = a.Uuid,
                    Title = a.Title,
                    Text = a.Text,
                    Author = a.Author,
                    Published = a.Published,
                    Categories = a.Categories.Select(c => c.Name).ToList(),
                    Thread = new GetArticle.ThreadInfoDto
                    {
                        Id = a.Thread.Id,
                        Site = a.Thread.Site,
                        Country = a.Thread.Country,
                        MainImage = a.Thread.MainImage,
                        DomainRank = a.Thread.DomainRank,
                        CreatedAt = a.Thread.CreatedAt
                    }
                })
                .ToListAsync(cancellationToken);
        }
    }
}