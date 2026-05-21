using ToDoApi.Models;

namespace ToDoApi.Repositories;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(int id);
    Task AddAsync(Contact contact);
    void Update(Contact contact);
    void Delete(Contact contact);
    Task<bool> SaveChangesAsync();
}