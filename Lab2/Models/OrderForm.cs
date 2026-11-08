namespace TasksApp.Models;

/// <summary>Data posted from the ShoppingCart form.</summary>
public class OrderForm
{
    public string CustomerName { get; set; } = "";
    public string Address { get; set; } = "";
    public string Supermarket { get; set; } = "";
    public string ShipDate { get; set; } = "";
    public List<string> Products { get; set; } = new();
}
