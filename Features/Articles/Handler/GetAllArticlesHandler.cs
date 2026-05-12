using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;
using ToDoApi.Features.Articles.Queries;

namespace ToDoApi.Features.Articles.Handlers;

public class GetAllArticlesHandler : IRequestHandler<GetAllArticlesQuery, List<Article>>
{
    private readonly AppDbContext _context;

    public GetAllArticlesHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Article>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Articles.ToListAsync(cancellationToken);
    }
}