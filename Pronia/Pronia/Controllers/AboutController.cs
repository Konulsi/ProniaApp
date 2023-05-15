using Microsoft.AspNetCore.Mvc;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAdvertisingService _advertisingService;
        private readonly IBrandService _brandService;
        private readonly IBlogService _blogService;


        public AboutController(AppDbContext context,
                                IAdvertisingService advertisingService,
                                IBlogService blogService,
                                IBrandService brandService)
        {
            _context = context;
            _advertisingService = advertisingService;
            _blogService = blogService;
            _brandService = brandService;    

        }

        public async Task<IActionResult> Index()
        {

            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Advertising> advertisings = await _advertisingService.GetAll();
            List<Brand> brands = await _brandService.GetBrands();
            List<Blog> blogs = await _blogService.GetBlogs();


            AboutVM model = new AboutVM()
            {
                HeaderBackgrounds= headerBackgrounds,
                Advertisings = advertisings,
                Brands= brands,
                Blogs= blogs
            };


            return View(model);
        }
    }
}
