using Microsoft.AspNetCore.Mvc.Rendering;

namespace TasksApp.Models;

public class ShoppingCartViewModel
{
    public string CustomerName { get; set; } = "";
    public string Address { get; set; } = "";
    public string ShipDate { get; set; } = "";

    public List<SelectListItem> Supermarkets { get; set; } = new();
    public List<SelectListItem> ShipDates { get; set; } = new();
    public List<SelectListItem> ProductItems { get; set; } = new();

    /// <summary>Result message shown after submit.</summary>
    public string? Message { get; set; }
}
