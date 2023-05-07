using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories();
    }
}
