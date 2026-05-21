using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;
using ToDoApi.Services.Contacts;

namespace ToDoApi.Services.Contacts;

public class ContactService(AppDbContext context) : IContactService
{
    public async Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return await context.Contacts.ToListAsync();
    }

    public async Task<Contact?> GetByIdAsync(int id)
    {
        return await context.Contacts.FindAsync(id);
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        context.Contacts.Add(contact);
        await context.SaveChangesAsync();
        return contact;
    }

    public async Task<bool> UpdateAsync(int id, Contact updatedContact)
    {
        var contact = await context.Contacts.FindAsync(id);
        if (contact == null) return false;

        contact.Name = updatedContact.Name;
        contact.Phone = updatedContact.Phone;
        contact.Email = updatedContact.Email;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var contact = await context.Contacts.FindAsync(id);
        if (contact == null) return false;

        context.Contacts.Remove(contact);
        await context.SaveChangesAsync();
        return true;
    }
}