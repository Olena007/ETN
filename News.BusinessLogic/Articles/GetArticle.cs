using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Interfaces;
using News.Entities;

namespace News.BusinessLogic.Articles;

public class GetArticle
{
    public class GetArticleQuery : IRequest<ArticleDto>
    {
        public Guid Id { get; set; }
    }

    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Uuid { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Author { get; set; }
        public DateTime Published { get; set; }
        public string? Language { get; set; }
        public string? Sentiment { get; set; }
        public string? Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ThreadInfoDto? Thread { get; set; }
        public List<CategoryDto> Categories { get; set; } = new();
        public List<ArticleUnitDto> Entities { get; set; } = new();
        public GroupedEntitiesDto GroupedEntities { get; set; } = new();
    }

    public class ThreadInfoDto
    {
        public Guid Id { get; set; }
        public string Site { get; set; } = null!;
        public string? Country { get; set; }
        public string? MainImage { get; set; }
        public int? DomainRank { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class ArticleUnitDto
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? Sentiment { get; set; }
    }

    public class GroupedEntitiesDto
    {
        public List<EntityItemDto> Persons { get; set; } = new();
        public List<EntityItemDto> Organizations { get; set; } = new();
        public List<EntityItemDto> Locations { get; set; } = new();
    }

    public class EntityItemDto
    {
        public string Name { get; set; } = null!;
        public string? Sentiment { get; set; }
    }

    public class GetArticleQueryHandler : IRequestHandler<GetArticleQuery, ArticleDto>
    {
        private readonly INewsDbContext _context;

        public GetArticleQueryHandler(INewsDbContext context)
        {
            _context = context;
        }

        public async Task<ArticleDto> Handle(GetArticleQuery request, CancellationToken cancellationToken)
        {
            var article = await _context.Articles
                .Include(a => a.Thread)
                .Include(a => a.Categories)
                .Include(a => a.Entities)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (article == null)
                throw new NotFoundException(nameof(Article), request.Id);

            return new ArticleDto
            {
                Id = article.Id,
                Uuid = article.Uuid,
                Title = article.Title,
                Text = article.Text,
                Author = article.Author,
                Published = article.Published,
                Language = article.Language,
                Sentiment = article.Sentiment,
                Url = article.Url,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,

                Thread = article.Thread != null
                    ? new ThreadInfoDto
                    {
                        Id = article.Thread.Id,
                        Site = article.Thread.Site,
                        Country = article.Thread.Country,
                        MainImage = article.Thread.MainImage,
                        DomainRank = article.Thread.DomainRank,
                        CreatedAt = article.Thread.CreatedAt
                    }
                    : null,

                Categories = article.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedAt = c.CreatedAt
                }).ToList(),

                Entities = article.Entities.Select(e => new ArticleUnitDto
                {
                    Name = e.Name,
                    Type = e.Type,
                    Sentiment = e.Sentiment
                }).ToList(),

                GroupedEntities = new GroupedEntitiesDto
                {
                    Persons = article.Entities
                        .Where(e => e.Type.Equals("person", StringComparison.OrdinalIgnoreCase))
                        .Select(e => new EntityItemDto
                        {
                            Name = e.Name,
                            Sentiment = e.Sentiment
                        }).ToList(),

                    Organizations = article.Entities
                        .Where(e => e.Type.Equals("organization", StringComparison.OrdinalIgnoreCase))
                        .Select(e => new EntityItemDto
                        {
                            Name = e.Name,
                            Sentiment = e.Sentiment
                        }).ToList(),

                    Locations = article.Entities
                        .Where(e => e.Type.Equals("location", StringComparison.OrdinalIgnoreCase))
                        .Select(e => new EntityItemDto
                        {
                            Name = e.Name,
                            Sentiment = e.Sentiment
                        }).ToList()
                }
            };
        }
    }
}