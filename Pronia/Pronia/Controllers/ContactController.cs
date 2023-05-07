using Microsoft.AspNetCore.Mvc;
using Pronia.Data;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);



            ContactVM model = new ContactVM()
            {
                HeaderBackgrounds = headerBackgrounds,
            };


            return View(model);
        }
    }
}
