using ProductsApp.Models;

namespace ProductsApp.Services;

public interface IProductRepository
{
    List<Product> GetAll();
    Product? Get(int id);
    Product Add(Product product);
    bool Update(Product product);
    bool Delete(int id);
}
