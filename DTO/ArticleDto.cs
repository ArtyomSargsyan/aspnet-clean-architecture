using System.ComponentModel.DataAnnotations;

namespace ToDoApi.DTO;

public record ArticleDto(int Id, string Title, string Content, DateTime CreatedAt);

public class ArticleCreateRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters.")]
    [MaxLength(150, ErrorMessage = "Title cannot exceed 150 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required.")]
    [MinLength(20, ErrorMessage = "Content must be at least 20 characters.")]
    public string Content { get; set; } = string.Empty;
}
