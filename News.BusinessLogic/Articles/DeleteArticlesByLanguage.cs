using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Articles;

public class DeleteArticlesByLanguage
{
    public class DeleteArticlesByLanguageCommand : IRequest<int>
    {
        public string Language { get; set; } = string.Empty;
    }

    public class DeleteArticlesByLanguageCommandHandler : IRequestHandler<DeleteArticlesByLanguageCommand, int>
    {
        private readonly INewsDbContext _context;

        public DeleteArticlesByLanguageCommandHandler(INewsDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteArticlesByLanguageCommand request, CancellationToken cancellationToken)
        {
            var articlesToDelete = await _context.Articles
                .Where(a => a.Language != null && a.Language.ToLower() == request.Language.ToLower())
                .ToListAsync(cancellationToken);

            if (!articlesToDelete.Any())
                return 0;

            _context.Articles.RemoveRange(articlesToDelete);
            await _context.SaveChangesAsync(cancellationToken);

            return articlesToDelete.Count;
        }
    }
}