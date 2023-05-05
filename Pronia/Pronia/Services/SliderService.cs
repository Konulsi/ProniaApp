using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        public SliderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Slider>> GetAll()
        {
            return await _context.Sliders.Where(m => !m.SoftDelete).ToListAsync();
        }
    }
}
