using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int? id);
    }
}
