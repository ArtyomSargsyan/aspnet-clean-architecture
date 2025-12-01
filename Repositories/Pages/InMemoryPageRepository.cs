
using ToDoApi.Models;
using ToDoApi.Repositories.Pages;
public class InMemoryPageRepository : IPageRepository
{
    private readonly List<Page> _pages = new();

    public Task<List<Page>> GetAllAsync()
        => Task.FromResult(_pages);

    public Task<Page?> GetByIdAsync(int id)
        => Task.FromResult(_pages.FirstOrDefault(p => p.Id == id));

    public Task AddAsync(Page page)
    {
        page.Id = _pages.Count + 1;
        _pages.Add(page);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Page page)
    {
        var existing = _pages.FirstOrDefault(p => p.Id == page.Id);
        if (existing != null)
        {
            existing.Title = page.Title;
            existing.Content = page.Content;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var page = _pages.FirstOrDefault(p => p.Id == id);
        if (page != null)
            _pages.Remove(page);

        return Task.CompletedTask;
    }
}
