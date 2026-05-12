using MediatR;
using ToDoApi.Data;
using ToDoApi.Exceptions;
using ToDoApi.Models;
using ToDoApi.Features.Articles.Commands;

namespace ToDoApi.Features.Articles.Handlers;

public class DeleteArticleHandler : IRequestHandler<DeleteArticleCommand, Unit>
{
    private readonly AppDbContext _context;

    public DeleteArticleHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(Article), request.Id);

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
