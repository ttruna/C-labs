namespace ProductsApp.Models;

public class ProductsIndexViewModel
{
    public List<Product> Products { get; set; } = new();
    public List<string> Categories { get; set; } = new();

    // current filter values (taken from the query string)
    public string? Name { get; set; }
    public string? Category { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
