using BnB.Api.Models;

namespace BnB.Api.Interfaces;

public interface IImageRepository
{
    Task<IEnumerable<string>> GetImagesByProductId(string productId);
    Task CreateImagesByProductId(string productId, IEnumerable<string> urls);
}