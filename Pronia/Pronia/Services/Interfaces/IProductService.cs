using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetById(int id);
        Task<List<Product>> GetAll();
        Task<Product> GetFullDataById(int? id);
        Task<List<Product>> GetFeaturedProducts();
        Task<List<Product>> GetBestsellerProducts();
        Task<List<Product>> GetLatestProducts();
        Task<List<Product>> GetNewProducts();
        Task<int> GetCountAsync();
        Task<List<Product>> GetPaginatedDatas(int page, int take);



    }
}
