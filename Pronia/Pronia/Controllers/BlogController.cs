using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ITagService _tagService;



        public BlogController(AppDbContext context,
                              IBlogService blogService,
                              ICategoryService categoryService,
                              IProductService productService,
                              ITagService tagService)
        {
            _context = context;
            _blogService = blogService;
            _categoryService = categoryService;
            _productService = productService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 2)
        {
            //paginated datas
            List<Blog> paginateBlog = await _blogService.GetPaginatedDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Blog> paginateDatas = new(paginateBlog, page, pageCount);


            List<Category> categories = await _categoryService.GetCategories();
            List<Product> products = await _productService.GetAll();
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Blog> blogs = await _blogService.GetBlogs();
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Tag> tags = await _tagService.GetAllAsync();


            BlogVM model = new BlogVM()
            {
                HeaderBackgrounds = headerBackgrounds,
                Blogs= blogs,
                PaginateBlog = paginateDatas,
                Categories = categories,
                Products= products,
                NewProducts= newProducts,
                Tags = tags
            };

            return View(model);
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _blogService.GetCountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
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



        public async Task<IActionResult> BlogDetail(int? id)
        {
            Blog blogDt = await _blogService.GetByIdAsync((int)id);
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Category> categories = await _categoryService.GetCategories();
            List<Blog> blogs = await _blogService.GetBlogs();
            List<Tag> tags = await _tagService.GetAllAsync();
            List<Product> newProduct = await _productService.GetNewProducts();


            List<BlogComment> blogComments = await _context.BlogComments.Include(m => m.AppUser).Where(m => m.BlogId == id).ToListAsync();
            CommentVM commentVM = new CommentVM();


            BlogDetailVM model = new()
            {
                BlogDt = blogDt,
                HeaderBackgrounds = headerBackgrounds,
                BlogComments = blogComments,
                CommentVM = commentVM,
                Categories = categories,
                Blogs = blogs,
                Tags = tags,
                NewProducts = newProduct
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PostComment(BlogDetailVM blogDetailVM, string userId, int blogId)
        {
            if (blogDetailVM.CommentVM.Message == null)
            {
                ModelState.AddModelError("Message", "Don't empty");
                return RedirectToAction(nameof(BlogDetail), new { id = blogId }); 
            }

            BlogComment blogComment = new()
            {
                FullName = blogDetailVM.CommentVM?.FullName,
                Email = blogDetailVM.CommentVM?.Email,
                Subject = blogDetailVM.CommentVM?.Subject,
                Message = blogDetailVM.CommentVM?.Message,
                AppUserId = userId,
                BlogId = blogId
            };

            await _context.BlogComments.AddAsync(blogComment);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(BlogDetail), new { id = blogId });

        }



    }
}
