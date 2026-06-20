using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.DTO;
using ToDoApi.Exceptions;
using ToDoApi.Models;

namespace ToDoApi.Services.Articles;

public class ArticleService : IArticleService
{
    private readonly AppDbContext _context;

    public ArticleService(AppDbContext context) => _context = context;

    public async Task<List<ArticleDto>> GetAllAsync()
        => await _context.Articles
            .Select(a => new ArticleDto(a.Id, a.Title, a.Content, a.CreatedAt))
            .ToListAsync();

    public async Task<ArticleDto> GetByIdAsync(int id)
    {
        var article = await _context.Articles.FindAsync(id)
            ?? throw new NotFoundException(nameof(Article), id);

        return new ArticleDto(article.Id, article.Title, article.Content, article.CreatedAt);
    }

    public async Task<ArticleDto> CreateAsync(ArticleCreateRequest request)
    {
        var article = new Article
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        return new ArticleDto(article.Id, article.Title, article.Content, article.CreatedAt);
    }

    public async Task DeleteAsync(int id)
    {
        var article = await _context.Articles.FindAsync(id)
            ?? throw new NotFoundException(nameof(Article), id);

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
    }
}
