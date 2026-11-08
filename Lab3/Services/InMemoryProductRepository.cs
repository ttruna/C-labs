using ProductsApp.Models;

namespace ProductsApp.Services;

public class InMemoryProductRepository : IProductRepository
{
    private readonly object _lock = new();
    private readonly List<Product> _items = new()
    {
        new Product { Id = 1, Name = "Milk",    Category = "Dairy",  Price = 42.50m,  Description = "Fresh milk, 1 l" },
        new Product { Id = 2, Name = "Bread",   Category = "Bakery", Price = 25.00m,  Description = "Wheat bread" },
        new Product { Id = 3, Name = "Eggs",    Category = "Dairy",  Price = 60.90m,  Description = "10 eggs" },
        new Product { Id = 4, Name = "Apples",  Category = "Fruits", Price = 35.75m,  Description = "Red apples, 1 kg" },
        new Product { Id = 5, Name = "Cheese",  Category = "Dairy",  Price = 180.00m, Description = "Hard cheese, 500 g" },
        new Product { Id = 6, Name = "Bananas", Category = "Fruits", Price = 48.20m,  Description = "1 kg" }
    };
    private int _nextId = 7;

    public List<Product> GetAll()
    {
        lock (_lock) return _items.Select(Clone).ToList();
    }

    public Product? Get(int id)
    {
        lock (_lock)
        {
            var p = _items.FirstOrDefault(x => x.Id == id);
            return p is null ? null : Clone(p);
        }
    }

    public Product Add(Product product)
    {
        lock (_lock)
        {
            product.Id = _nextId++;
            _items.Add(Clone(product));
            return product;
        }
    }

    public bool Update(Product product)
    {
        lock (_lock)
        {
            var existing = _items.FirstOrDefault(x => x.Id == product.Id);
            if (existing is null) return false;
            existing.Name = product.Name;
            existing.Category = product.Category;
            existing.Price = product.Price;
            existing.Description = product.Description;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock) return _items.RemoveAll(x => x.Id == id) > 0;
    }

    private static Product Clone(Product p) => new()
    {
        Id = p.Id, Name = p.Name, Category = p.Category, Price = p.Price, Description = p.Description
    };
}
