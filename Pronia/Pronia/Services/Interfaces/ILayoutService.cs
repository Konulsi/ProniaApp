using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Services.Interfaces
{
    public interface ILayoutService
    {
        Dictionary<string, string> GetSettingsData();

        Task<IEnumerable<Social>> GetSocialData();
    }
}
