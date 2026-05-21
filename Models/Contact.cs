using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Models;

public class Contact
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string? Email { get; set; } 
}