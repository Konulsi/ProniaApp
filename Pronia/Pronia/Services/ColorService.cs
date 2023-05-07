using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;


namespace Pronia.Services
{
    public class ColorService : IColorService
    {
        private readonly AppDbContext _context;

        public ColorService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Color>> GetAllColors()
        {
            return await _context.Colors.Include(m=>m.Products).Where(m => !m.SoftDelete).ToListAsync();
        }

     
    }
}
