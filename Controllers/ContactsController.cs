using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;
using ToDoApi.Services.Contacts;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(IContactService contactService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contact>>> GetContact()
    {
        return Ok(await contactService.GetContactsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await contactService.GetByIdAsync(id);
        return contact == null ? NotFound(new { message = "Contact not found" }) : Ok(contact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, Contact updatedContact)
    {
        var updated = await contactService.UpdateAsync(id, updatedContact);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> CreateContact(Contact contact)
    {
        var result = await contactService.CreateAsync(contact);

        return Ok("Contact created successfully");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var deleted = await contactService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}

