using ToDoApi.DTO;

namespace ToDoApi.Services.Articles;

public interface IArticleService
{
    Task<List<ArticleDto>> GetAllAsync();
    Task<ArticleDto> GetByIdAsync(int id);
    Task<ArticleDto> CreateAsync(ArticleCreateRequest request);
    Task DeleteAsync(int id);
}
