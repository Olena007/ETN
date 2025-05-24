using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Common.Exeptions;
using News.BusinessLogic.Interfaces;
using News.BusinessLogic.Users;

namespace News.BusinessLogic.News;

public class GetSingleNewsQueryHandler: IRequestHandler<GetSingleNewsQuery, NewsModel>
{
    private readonly INewsDbContext _context;
    private readonly IMapper _mapper;

    public GetSingleNewsQueryHandler(INewsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NewsModel> Handle(GetSingleNewsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.News
            .FirstOrDefaultAsync(x => x.NewsId == request.NewsId);

        if (entity == null)
            throw new NotFoundException(nameof(News), request.NewsId);

        return _mapper.Map<NewsModel>(entity);
    }
}

public class GetSingleNewsQuery : IRequest<NewsModel>
{
    public Guid NewsId { get; set; }
}