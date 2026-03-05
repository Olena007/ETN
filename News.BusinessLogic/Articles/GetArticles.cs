using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Articles;

public class GetArticles
{
    public class GetArticlesQuery : IRequest<ArticlesListDto>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Language { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ArticlesListDto
    {
        public List<ArticleListItemDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public class ArticleListItemDto
    {
        public Guid Id { get; set; }
        public string Uuid { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Author { get; set; }
        public DateTime Published { get; set; }
        public string? Language { get; set; }
        public string? Sentiment { get; set; }
        public string Site { get; set; } = null!;
        public List<string> Categories { get; set; } = new();
    }

    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, ArticlesListDto>
    {
        private readonly INewsDbContext _context;

        public GetArticlesQueryHandler(INewsDbContext context)
        {
            _context = context;
        }

        public async Task<ArticlesListDto> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Articles
                .Include(a => a.Thread)
                .Include(a => a.Categories)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(a =>
                    a.Title.ToLower().Contains(searchTerm) ||
                    a.Text.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(request.Category))
                query = query.Where(a =>
                    a.Categories.Any(c => c.Name == request.Category));

            if (!string.IsNullOrWhiteSpace(request.Language)) query = query.Where(a => a.Language == request.Language);

            if (request.FromDate.HasValue) query = query.Where(a => a.Published >= request.FromDate.Value);

            if (request.ToDate.HasValue) query = query.Where(a => a.Published <= request.ToDate.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(a => a.Published)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new ArticleListItemDto
                {
                    Id = a.Id,
                    Uuid = a.Uuid,
                    Title = a.Title,
                    Author = a.Author,
                    Published = a.Published,
                    Language = a.Language,
                    Sentiment = a.Sentiment,
                    Site = a.Thread != null ? a.Thread.Site : string.Empty,
                    Categories = a.Categories.Select(c => c.Name).ToList()
                })
                .ToListAsync(cancellationToken);

            return new ArticlesListDto
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}