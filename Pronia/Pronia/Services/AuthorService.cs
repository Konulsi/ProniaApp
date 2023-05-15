using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;

        public AuthorService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Author>> GetAllAsync() => await _context.Authors.Include(m => m.Blogs).Where(m => !m.SoftDelete).ToListAsync();

        public async Task<Author> GetByIdAsync(int? id) => await _context.Authors.Include(m => m.Blogs).Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);


    }
}
