using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Articles;

public class GetLanguages
{
    public class GetLanguagesQuery : IRequest<List<string>>
    {
    }

    public class GetLanguagesQueryHandler : IRequestHandler<GetLanguagesQuery, List<string>>
    {
        private readonly INewsDbContext _context;

        public GetLanguagesQueryHandler(INewsDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
        {
            return (await _context.Articles
                .Where(a => a.Language != null)
                .Select(a => a.Language)
                .Distinct()
                .OrderBy(l => l)
                .ToListAsync(cancellationToken))!;
        }
    }
}