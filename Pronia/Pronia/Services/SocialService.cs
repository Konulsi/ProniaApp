using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class SocialService : ISocialService
    {

        private readonly AppDbContext _context;

        public SocialService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Social>> GetAllSocials() => await _context.Socials.Where(m => !m.SoftDelete).ToListAsync();

        public async Task<Social> GetSocialById(int? id) => await _context.Socials.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);

    }
}
