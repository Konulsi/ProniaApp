using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<Blog>> GetBlogs();
    }
}
