using BnB.Api.Data;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BnB.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _db;

    public CategoryRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<Category>> GetCategories()
    {
        var categories = await _db.Categories.ToListAsync();
        return categories;
    }
}