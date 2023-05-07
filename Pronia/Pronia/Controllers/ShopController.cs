using Microsoft.AspNetCore.Mvc;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IColorService _colorService;


        public ShopController(AppDbContext context,IProductService productService, IColorService colorService, ICategoryService categoryService)
        {
            _context = context;
            _productService = productService;   
            _colorService = colorService;
            _categoryService = categoryService;

        }

        public async Task<IActionResult>  Index()
        {

            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Category> categories = await _categoryService.GetCategories();
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Color> colors = await _colorService.GetAllColors();


            ShopVM model = new ShopVM()
            {
                HeaderBackgrounds = headerBackgrounds,
                Categories = categories,
                NewProducts= newProducts,
                Colors = colors
            };


            return View(model);
        }
    }
}
