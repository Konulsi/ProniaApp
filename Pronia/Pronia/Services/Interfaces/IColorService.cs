

using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IColorService
    {
        Task<List<Color>> GetAllColors();
    }
}
