using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface ISocialService
    {
        Task<List<Social>> GetAllSocials();

        Task<Social> GetSocialById(int? id);
    }
}
