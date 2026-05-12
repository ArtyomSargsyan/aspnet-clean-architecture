using MediatR;
using ToDoApi.Data;
using ToDoApi.Models;
using ToDoApi.Features.Articles.Commands;

namespace ToDoApi.Features.Articles.Handlers;

public class CreateArticleHandler : IRequestHandler<CreateArticleCommand, int>
{
    private readonly AppDbContext _context;

    public CreateArticleHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = new Article
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.Articles.Add(article);
        await _context.SaveChangesAsync(cancellationToken);

        return article.Id;
    }
}