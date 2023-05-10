using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<Blog>> GetBlogs();
        Task<int> GetCountAsync();
        Task<List<Blog>> GetPaginatedDatas(int page, int take);
    }
}
