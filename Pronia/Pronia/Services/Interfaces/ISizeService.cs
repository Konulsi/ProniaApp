using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface ISizeService
    {
        Task<List<Size>> GetAllSize();
        Task<Size> GetById(int? id);
    }
}
