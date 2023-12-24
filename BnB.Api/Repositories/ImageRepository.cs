using BnB.Api.Data;
using BnB.Api.Interfaces;
using BnB.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BnB.Api.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly ApplicationDbContext _db;

    public ImageRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    

    public async Task<IEnumerable<string>> GetImagesByProductId(string productId)
    {
        var images = await _db.Images.Where(i => i.ProductId == productId).Select(i => i.Url).ToListAsync();
        return images;
    }

    public async Task CreateImagesByProductId(string productId, IEnumerable<string> urls)
    {
        var images = new List<Image>();
        foreach (var url in urls)
        {
            images.Add(new Image
            {
                ProductId = productId,
                Url = url,
                Id = Guid.NewGuid().ToString()
            });
        }

        await _db.Images.AddRangeAsync(images);

        await _db.SaveChangesAsync();
    }
}