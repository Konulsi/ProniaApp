using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IBrandService
    {
        Task<List<Brand>> GetBrands();
    }
}
