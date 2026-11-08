using System.ComponentModel.DataAnnotations;

namespace ProductsApp.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Category is required")]
    [StringLength(50)]
    public string Category { get; set; } = "";

    [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}
