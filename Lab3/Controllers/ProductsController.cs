using Microsoft.AspNetCore.Mvc;
using ProductsApp.Models;
using ProductsApp.Services;

namespace ProductsApp.Controllers;

// Both prefixes work: /products/... and /items/...
[Route("products")]
[Route("items")]
public class ProductsController : Controller
{
    private readonly IProductRepository _repo;

    public ProductsController(IProductRepository repo) => _repo = repo;

    private IActionResult ProductNotFoundRedirect()
        => RedirectToAction("ProductNotFound", "Error");

    // /products, /products/, /products/index, /items, /items/index
    // Filters from the query string: ?name=..&category=..&minPrice=..&maxPrice=..
    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index(string? name, string? category, decimal? minPrice, decimal? maxPrice)
    {
        var all = _repo.GetAll();
        IEnumerable<Product> query = all;

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name.Trim(), StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => string.Equals(p.Category, category.Trim(), StringComparison.OrdinalIgnoreCase));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        var vm = new ProductsIndexViewModel
        {
            Products = query.OrderBy(p => p.Id).ToList(),
            Categories = all.Select(p => p.Category).Distinct().OrderBy(c => c).ToList(),
            Name = name,
            Category = category,
            MinPrice = minPrice,
            MaxPrice = maxPrice
        };
        return View(vm);
    }

    // /products/5
    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var product = _repo.Get(id);
        if (product is null) return ProductNotFoundRedirect();
        return View(product);
    }

    // /products/create and /products/new
    [HttpGet("create")]
    [HttpGet("new")]
    public IActionResult Create() => View(new Product());

    [HttpPost("create")]
    [HttpPost("new")]
    public IActionResult Create([Bind("Name,Category,Price,Description")] Product product)
    {
        if (!ModelState.IsValid) return View(product);
        _repo.Add(product);
        return RedirectToAction(nameof(Index));
    }

    // /products/edit/5
    [HttpGet("edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var product = _repo.Get(id);
        if (product is null) return ProductNotFoundRedirect();
        return View(product);
    }

    [HttpPost("edit/{id:int}")]
    public IActionResult Edit(int id, [Bind("Name,Category,Price,Description")] Product product)
    {
        if (_repo.Get(id) is null) return ProductNotFoundRedirect();

        product.Id = id;
        if (!ModelState.IsValid) return View(product);

        _repo.Update(product);
        return RedirectToAction(nameof(Index));
    }

    // /products/delete/5  (confirmation page)
    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        var product = _repo.Get(id);
        if (product is null) return ProductNotFoundRedirect();
        return View(product);
    }

    [HttpPost("delete/{id:int}")]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!_repo.Delete(id)) return ProductNotFoundRedirect();
        return RedirectToAction(nameof(Index));
    }
}
