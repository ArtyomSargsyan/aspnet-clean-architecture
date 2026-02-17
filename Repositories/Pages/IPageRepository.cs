using ToDoApi.Models;   
namespace ToDoApi.Repositories.Pages;
public interface IPageRepository
{
    Task<List<Page>> GetAllAsync();
    Task<Page?> GetByIdAsync(int id);
    Task AddAsync(Page page);
    Task UpdateAsync(Page page);
    Task DeleteAsync(int id);
}
