using BnB.Api.Dto.Product;
using BnB.Api.Models;

namespace BnB.Api.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts();
    Task<Product> GetProductById(string productId);
    Task<Product> CreateProduct(Product createProduct);
    Task<Product> UpdateProduct(string productId, Product updateProduct);
    Task<bool> DeleteProductById(string productId);
}