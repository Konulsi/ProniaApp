

using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IColorService
    {
        Task<Color> GetByIdAsync(int? id);
        Task<List<Color>> GetAllColors();
    }
}
