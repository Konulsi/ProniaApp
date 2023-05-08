using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Tag>> GetAllAsync() => await _context.Tags.Include(m=>m.ProductTags).Where(m => !m.SoftDelete).ToListAsync();
    }
}
