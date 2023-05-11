using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IBannerService
    {
        Task<List<Banner>> GetAllAsync();

        Task<Banner> GetByIdAsync(int? id);
    }
}
