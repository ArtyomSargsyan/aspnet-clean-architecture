using MediatR;
using ToDoApi.Data;
using ToDoApi.Exceptions;
using ToDoApi.Features.Articles.Queries;
using ToDoApi.Models;

namespace ToDoApi.Features.Articles.Handlers;

public class GetArticleByIdHandler : IRequestHandler<GetArticleByIdQuery, Article>
{
    private readonly AppDbContext _context;

    public GetArticleByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Article> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.FindAsync([request.Id], cancellationToken);

        if (article == null)
            throw new NotFoundException("Article", request.Id);

        return article;
    }
}
