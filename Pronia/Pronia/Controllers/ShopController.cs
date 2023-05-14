using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
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
        private readonly ITagService _tagService;
        private readonly IAdvertisingService _advertisingService;



        public ShopController(AppDbContext context,
                              IProductService productService,
                              IColorService colorService,
                              ICategoryService categoryService,
                              ITagService tagService,
                              IAdvertisingService advertisingService)
        {
            _context = context;
            _productService = productService;
            _colorService = colorService;
            _categoryService = categoryService;
            _tagService = tagService;
            _advertisingService = advertisingService;

        }

        public async Task<IActionResult> Index(int page = 1, int take = 5, int? cateId = null, int? tagId = null)
        {
            List<Product> paginateProduct = await _productService.GetPaginatedDatas(page, take, cateId, tagId);
            int pageCount = await GetPageCountAsync(take);

            Paginate<Product> paginateDatas = new(paginateProduct, page, pageCount);


            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Category> categories = await _categoryService.GetCategories();
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Color> colors = await _colorService.GetAllColors();
            List<Tag> tags = await _tagService.GetAllAsync();






            ShopVM model = new ShopVM()
            {
                HeaderBackgrounds = headerBackgrounds,
                Categories = categories,
                NewProducts = newProducts,
                Colors = colors,
                PaginateProduct = paginateDatas,
                Tags = tags
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
            List<Product> products = await _context.ProductCategories.Include(m => m.Product).ThenInclude(m => m.Images).Where(m => m.Category.Id == id).Select(m => m.Product).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }


        public async Task<IActionResult> GetAllProduct()
        {
            List<Product> products = await _productService.GetAll();

            return PartialView("_ProductsPartial", products);
        }


        public async Task<IActionResult> GetProductByColor(int? id)
        {
            List<Product> products = await _context.Products.Include(m => m.Color).Include(m => m.Images).Where(m => m.Color.Id == id).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }



        public async Task<IActionResult> GetProductsByTag(int? id)
        {
            List<Product> products = await _context.ProductTags.Include(m => m.Product).ThenInclude(m => m.Images).Where(m => m.Tag.Id == id).Select(m => m.Product).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }


        public async Task<IActionResult> MainSearch(string searchText)
        {
            var products = await _context.Products
                                .Include(m => m.Images)
                                .Include(m => m.ProductCategories)?
                                 .Include(m => m.ProductSizes)
                                 .Include(m => m.ProductTags)
                                 .Include(m => m.ProductComments)
                                .Where(m => !m.SoftDelete && m.Name.ToLower().Trim().Contains(searchText.ToLower().Trim()))
                                .Take(6)
                                .ToListAsync();

            return View(products);
        }



        public async Task<IActionResult> Search(string searchText)
        {
            List<Product> products = await _context.Products.Include(m => m.Images)
                                            .Include(m => m.ProductCategories)
                                            .Include(m => m.ProductSizes)
                                            .Include(m => m.ProductTags)
                                            .Include(m => m.ProductComments)
                                            .Where(m => m.Name.ToLower().Contains(searchText.ToLower()))
                                            .Take(5)
                                            .ToListAsync();
            return PartialView("_SearchPartial", products);
        }



        public async Task<IActionResult> ProductDetail(int? id)
        {
            Product productDt = await _productService.GetFullDataById((int)id);
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Advertising> advertisings = await _advertisingService.GetAll();
            List<Category> categories = await _categoryService.GetCategories();
            List<Product> releatedProducts = new();

            List<ProductComment> productComments = await _context.ProductComments.Include(m => m.AppUser).Where(m => m.ProductId == id).ToListAsync();
            CommentVM commentVM = new CommentVM();

            foreach (var category in categories)
            {
                Product releatedProduct = await _context.ProductCategories.Where(m => m.Category.Id == category.Id).Select(m => m.Product).FirstAsync();
                releatedProducts.Add(releatedProduct);
            }

            ProductDetailVM model = new()
            {
                ProductDt = productDt,
                HeaderBackgrounds = headerBackgrounds,
                Advertisings = advertisings,
                RelatedProducts = releatedProducts,
                CommentVM = commentVM,
                ProductComments = productComments
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PostComment(ProductDetailVM productDetailVM, string userId, int productId)
        {
            if (productDetailVM.CommentVM.Message == null)
            {
                ModelState.AddModelError("Message", "Don't empty");
                return RedirectToAction(nameof(ProductDetail), new { id = productId }); 
            }

            ProductComment productComment = new()
            {
                FullName = productDetailVM.CommentVM?.FullName,
                Email = productDetailVM.CommentVM?.Email,
                Subject = productDetailVM.CommentVM?.Subject,
                Message = productDetailVM.CommentVM?.Message,
                AppUserId = userId,
                ProductId = productId
            };

            await _context.ProductComments.AddAsync(productComment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProductDetail), new { id = productId });

        }







    }
}
