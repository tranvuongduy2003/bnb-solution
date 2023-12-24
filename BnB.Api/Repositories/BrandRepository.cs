using BnB.Api.Data;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BnB.Api.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly ApplicationDbContext _db;

    public BrandRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<Brand>> GetBrands()
    {
        var brands = await _db.Brands.ToListAsync();
        return brands;
    }
}