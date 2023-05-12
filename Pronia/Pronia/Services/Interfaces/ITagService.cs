using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int? id);
    }
}
