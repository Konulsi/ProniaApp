using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using System.Runtime.InteropServices;

namespace Pronia.Services
{
    public class BannerService : IBannerService
    {
        private readonly AppDbContext _context;
        public BannerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Banner>> GetAllAsync()
        {
            return await _context.Banners.Where(m => !m.SoftDelete).ToListAsync();

        }

        public async Task<Banner> GetByIdAsync(int? id)
        {
            return await _context.Banners.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);

        }
    }
}
