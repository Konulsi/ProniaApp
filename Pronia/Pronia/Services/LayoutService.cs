using Pronia.Data;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public LayoutVM GetSettingsData()
        {
            Dictionary<string, string> settings = _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);

            
            return new LayoutVM { Settings = settings };
        }
    }
}
