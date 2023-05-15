using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface ISettingService
    {
        List<Settings> GetSettingsAsync();

        Task<Settings> GetSettingByIdAsync(int? id);

    }
}
