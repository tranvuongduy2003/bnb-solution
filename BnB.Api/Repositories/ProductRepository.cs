using BnB.Api.Data;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BnB.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var products = await _db.Products.ToListAsync();
        return products;
    }

    public async Task<Product> GetProductById(string productId)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
        return product;
    }

    public async Task<Product> CreateProduct(Product createProduct)
    {
        createProduct.Id = Guid.NewGuid().ToString();
        createProduct.CreatedAt = DateTime.Now;
        createProduct.UpdatedAt = DateTime.Now;
        createProduct.Sold = 0;
        _db.Products.Add(createProduct);
        await _db.SaveChangesAsync();
        return createProduct;
    }

    public async Task<Product> UpdateProduct(string productId, Product updateProduct)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
        {
            return null;
        }
        
        product.BrandId = updateProduct.BrandId;
        product.Desc = updateProduct.Desc;
        product.Inventory = updateProduct.Inventory;
        product.CategoryId = updateProduct.CategoryId;
        product.Name = updateProduct.Name;
        product.Price = updateProduct.Price;
        product.Sold = updateProduct.Sold;
        product.ImportPrice = updateProduct.ImportPrice;
        product.UpdatedAt = DateTime.Now;

        await _db.SaveChangesAsync();
        
        return updateProduct;
    }

    public async Task<bool> DeleteProductById(string productId)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
        {
            return false;
        }
        
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        
        return true;
    }
}