using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Articles;

public abstract class DeleteArticle
{
    public class DeleteArticleCommand : IRequest<bool> {
        public Guid Id { get; set; }
    }

    public class DeleteArticleCommandHandler(INewsDbContext context) : IRequestHandler<DeleteArticleCommand, bool>
    {
        public async Task<bool> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await context.Articles
                .FirstOrDefaultAsync(a => a.Id  == request.Id, cancellationToken: cancellationToken);

            if (article == null)
                return false;
            
            context.Articles.Remove(article);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}