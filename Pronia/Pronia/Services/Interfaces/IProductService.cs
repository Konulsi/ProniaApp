using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetById(int id);
        Task<List<Product>> GetAll();
        Task<int> GetCountAsync();
        Task<Product> GetFullDataById(int id);
        Task<List<Product>> GetFeaturedProducts();
        Task<List<Product>> GetBestsellerProducts();
        Task<List<Product>> GetLatestProducts();

    }
}
