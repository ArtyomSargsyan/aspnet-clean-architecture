using ToDoApi.Models;

namespace ToDoApi.Services.Contacts;

public interface IContactService
{
    Task<IEnumerable<Contact>> GetContactsAsync();
    Task<Contact?> GetByIdAsync(int id);
    Task<Contact> CreateAsync(Contact contact);
    Task<bool> UpdateAsync(int id, Contact updateContact);
    Task<bool> DeleteAsync(int id);
}

