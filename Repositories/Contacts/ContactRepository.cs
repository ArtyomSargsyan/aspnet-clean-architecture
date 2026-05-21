using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Repositories;

public class ContactRepository(AppDbContext context) : IContactRepository
{
    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        return await context.Contacts.AsNoTracking().ToListAsync();
    }

    public async Task<Contact?> GetByIdAsync(int id)
    {
        return await context.Contacts.FindAsync(id);
    }

    public async Task AddAsync(Contact contact)
    {
        await context.Contacts.AddAsync(contact);
    }

    public void Update(Contact contact)
    {
        context.Contacts.Update(contact);
    }

    public void Delete(Contact contact)
    {
        context.Contacts.Remove(contact);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}