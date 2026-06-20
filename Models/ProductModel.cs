namespace ToDoApi.Models;
using System.ComponentModel.DataAnnotations;

public class ProductModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public int ProductId { get; set; }
    public Product? Product { get; set; }
}