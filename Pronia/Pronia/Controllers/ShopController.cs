using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Helpers;
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

        public async Task<IActionResult>  Index(int page = 1, int take = 4)
        {
            List<Product> paginateProduct = await _productService.GetPaginatedDatas(page, take);
            int pageCount = await GetPageCountAsync(take);

            Paginate<Product> paginateDatas = new(paginateProduct, page, pageCount);


            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Category> categories = await _categoryService.GetCategories();
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Color> colors = await _colorService.GetAllColors();


     


            ShopVM model = new ShopVM()
            {
                HeaderBackgrounds = headerBackgrounds,
                Categories = categories,
                NewProducts= newProducts,
                Colors = colors,
                PaginateProduct = paginateDatas
            };


            return View(model);
        }




        private async Task<int> GetPageCountAsync(int take) 
        {
            var productCount = await _productService.GetCountAsync();  

            return (int)Math.Ceiling((decimal)productCount / take);  
        }



        public async Task<IActionResult> GetProductsByCategory(int? id)
        {
            List<Product> products = await _context.ProductCategories.Where(m => m.Category.Id == id).Select(m => m.Product).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }

        public async Task<IActionResult> GetAllProduct(int? id)
        {
            List<Product> products = await _productService.GetAll();

            return PartialView("_ProductsPartial", products);
        }


        public async Task<IActionResult> GetProductByColor(int? id)
        {
            List<Product> products = await _context.Products.Include(m=>m.Color).Where(m => m.Color.Id == id).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }


        public IActionResult Search(string searchText)
        {
            var products =_context.Products
                                .Include(m => m.Images)
                                .Include(m => m.ProductCategories)?
                                .OrderByDescending(m => m.Id)
                                .Where(m => !m.SoftDelete && m.Name.ToLower().Contains(searchText.ToLower()))
                                .Take(6)
                                .ToList();

                          


            return PartialView("_SearchPartial", products);
        }




    }
}
