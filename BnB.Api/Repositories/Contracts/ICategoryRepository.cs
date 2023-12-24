using BnB.Api.Models;

namespace BnB.Api.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategories();
}