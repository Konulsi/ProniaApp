using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IHeaderBackgroundService
    {
        List<HeaderBackground> GetHeaderBackgroundsAsync();

        Task<HeaderBackground> GetHeaderBackgroundByIdAsync(int? id);
    }
}
