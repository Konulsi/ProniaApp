using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services.Interfaces;


namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly ITagService _tagService;



        public ProductController(AppDbContext context, 
                                IWebHostEnvironment env, 
                                IProductService productService, 
                                ICategoryService categoryService,
                                ISizeService sizeService,
                                ITagService tagService,
                                IColorService colorService)
        {
            _context = context;
            _env = env;
            _productService = productService;
            _categoryService = categoryService;
            _sizeService = sizeService;
            _tagService = tagService;
            _colorService = colorService;

        }
        public async Task<IActionResult> Index(int page = 1, int take = 5 , int? cateId = null)
        {
            List<Product> products = await _productService.GetPaginatedDatas(page, take , cateId);

            List<ProductListVM> mappedDatas = GetMappedDatas(products);
            int pageCount = await GetPageCountAsync(take);

            Paginate<ProductListVM> paginatedDatas = new(mappedDatas, page, pageCount);

            ViewBag.take = take;

            return View(paginatedDatas);
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Product dbProduct = await _productService.GetFullDataById((int)id);

                if (dbProduct == null) return NotFound();


                ProductDetailVM model = new()
                {
                    Name = dbProduct.Name,
                    SaleCount = dbProduct.SaleCount,
                    StockCount = dbProduct.StockCount,
                    Description= dbProduct.Description,
                    Price = dbProduct.Price,
                    SKU= dbProduct.SKU,
                    Rate= dbProduct.Rate,
                    ProductCategories = dbProduct.ProductCategories,
                    ProductImages = dbProduct.Images,
                    ProductSizes= dbProduct.ProductSizes,
                    ProductTags = dbProduct.ProductTags,
                    MainImage= dbProduct.MainImage,
                    HoverImage= dbProduct.HoverImage,
                    ColorName = dbProduct.Color.Name
                };

                return View(model);
            }
            catch (Exception)
            {
                throw;
            }

        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();
            return (int)Math.Ceiling((decimal)productCount / take);
        }

        private List<ProductListVM> GetMappedDatas(List<Product> products)
        {
            List<ProductListVM> mappedDatas = new();

            foreach (var product in products)
            {
                ProductListVM productVM = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    MainImage = product.MainImage,
                    SKU= product.SKU,
                };

                mappedDatas.Add(productVM);
            }

            return mappedDatas;
        }





        private async Task<SelectList> GetCategoryAsync()
        {
            List<Category> categories = await _categoryService.GetCategories();
            return new SelectList(categories, "Id", "Name");
        }

        private async Task<SelectList> GetColorAsync()
        {
            List<Color> colors = await _colorService.GetAllColors();
            return new SelectList(colors, "Id", "Name");
        }

        private async Task<SelectList> GetSizeAsync()
        {
            List<Size> sizes = await _sizeService.GetAllSize();
            return new SelectList(sizes, "Id", "Name");
        }

        private async Task<SelectList> GetTagAsync()
        {
            List<Tag> tags = await _tagService.GetAllAsync();
            return new SelectList(tags, "Id", "Name");
        }




    }
}
