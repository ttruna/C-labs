using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TasksApp.Data;
using TasksApp.Models;

namespace TasksApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult SprintTasks() => View();

    public IActionResult Greetings() => View();

    public IActionResult ProductInfo() => View();

    public IActionResult SuperMarkets()
    {
        ViewBag.Supermarkets = DemoData.Supermarkets.ToList();
        return View();
    }

    public IActionResult ShoppingList()
        => View(new Dictionary<string, int>(DemoData.ShoppingList));

    [HttpGet]
    public IActionResult ShoppingCart() => View(BuildCart(null));

    [HttpPost]
    public IActionResult ShoppingCart(OrderForm order)
    {
        var vm = BuildCart(order);

        if (string.IsNullOrWhiteSpace(order.CustomerName) || order.Products.Count == 0)
        {
            vm.Message = "Please enter your name and choose at least one product.";
        }
        else
        {
            var items = order.Products.Select(p =>
                DemoData.ShoppingList.TryGetValue(p, out int q) ? $"{p} x{q}" : p);

            string date = DateTime.TryParse(order.ShipDate, out var d)
                ? d.ToString("dddd, d MMMM yyyy")
                : order.ShipDate;

            vm.Message =
                $"Thank you, {order.CustomerName}! Your order ({string.Join(", ", items)}) " +
                $"from {order.Supermarket} will be shipped to \"{order.Address}\" on {date}.";
        }

        return View(vm);
    }

    private static ShoppingCartViewModel BuildCart(OrderForm? form)
    {
        form ??= new OrderForm();
        DateTime today = DateTime.Today;

        return new ShoppingCartViewModel
        {
            CustomerName = form.CustomerName,
            Address = form.Address,
            ShipDate = string.IsNullOrEmpty(form.ShipDate)
                ? today.ToString("yyyy-MM-dd")
                : form.ShipDate,

            Supermarkets = DemoData.Supermarkets
                .Select(s => new SelectListItem(s, s, s == form.Supermarket))
                .ToList(),

            // radio buttons: today and the next two days
            ShipDates = Enumerable.Range(0, 3)
                .Select(i => today.AddDays(i))
                .Select(d => new SelectListItem(d.ToString("dddd, d MMMM"), d.ToString("yyyy-MM-dd")))
                .ToList(),

            // list box: keys of the shopping list dictionary
            ProductItems = DemoData.ShoppingList.Keys
                .Select(k => new SelectListItem(k, k, form.Products.Contains(k)))
                .ToList()
        };
    }
}
