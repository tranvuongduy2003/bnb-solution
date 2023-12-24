using BnB.Api.Models;

namespace BnB.Api.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetBrands();
}