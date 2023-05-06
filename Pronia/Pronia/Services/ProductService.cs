using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class ProductService: IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAll() => await _context.Products.Include(m => m.Images).ToListAsync();
        public async Task<Product> GetFullDataById(int id) => await _context.Products
                                                                            .Include(m => m.Images)
                                                                            .Include(m => m.ProductSizes)
                                                                            .Include(m => m.ProductTags)
                                                                            .Include(m => m.Color)
                                                                            .Include(m => m.Comments)
                                                                            .Include(m => m.ProductCategories)?
                                                                            .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<Product> GetById(int id) => await _context.Products.FindAsync(id);
        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();

    }
}
