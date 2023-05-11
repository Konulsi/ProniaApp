using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IBlogService _blogService;
        private readonly IAuthorService _authorService;

        public BlogController(AppDbContext context,
                              IWebHostEnvironment env, 
                              IBlogService blogService, 
                              IAuthorService authorService)
        {
            _context = context;
            _env = env;
            _blogService = blogService;
            _authorService = authorService;
        }

        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _blogService.GetBlogs();
            return View(blogs);
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Blog dbBlog = await _blogService.GetByIdAsync(id);
            if (dbBlog is null) return NotFound();

            return View(dbBlog);
        }


        private async Task<SelectList> GetAuthorsAsync()
        {
            List<Author> authors = await _authorService.GetAllAsync();
            return new SelectList(authors, "Id", "Name");
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.authors = await GetAuthorsAsync();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM model)
        {
            try
            {
                ViewBag.authors = await GetAuthorsAsync();

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                foreach (var photo in model.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(model);
                    }

                    if (!photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }
                }

                List<BlogImage> blogImages = new();

                foreach (var photo in model.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(path, photo);

                    BlogImage blogImage = new()
                    {
                        Image = fileName
                    };

                    blogImages.Add(blogImage);
                }

                Blog newBlog = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    AuthorId = model.AuthorId,
                    Images = blogImages
                };

                await _context.BlogImages.AddRangeAsync(blogImages);
                await _context.Blogs.AddAsync(newBlog);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                Blog dbBlog = await _blogService.GetByIdAsync(id);
                if (dbBlog is null) return NotFound();

                ViewBag.authors = await GetAuthorsAsync();


                foreach (var item in dbBlog.Images)
                {
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", item.Image);

                    FileHelper.DeleteFile(path);

                }


                _context.Blogs.Remove(dbBlog);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            ViewBag.authors = await GetAuthorsAsync();


            Blog dbBlog = await _blogService.GetByIdAsync((int)id);

            if (dbBlog == null) return NotFound();


            BlogUpdateVM model = new()
            {
                Title = dbBlog.Title,
                AuthorId = dbBlog.AuthorId,
                Images = dbBlog.Images.ToList(),
                Description = dbBlog.Description
            };


            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BlogUpdateVM updatedBlog)
        {

            if (id == null) return BadRequest();

            ViewBag.authors = await GetAuthorsAsync();

            Blog dbBlog = await _blogService.GetByIdAsync(id);

            if (dbBlog == null) return NotFound();

            BlogUpdateVM model = new()
            {
                Title = dbBlog.Title,
                AuthorId = dbBlog.AuthorId,
                Images = dbBlog.Images.ToList(),
                Description = dbBlog.Description
            };


            if (!ModelState.IsValid)
            {
                model.Images = dbBlog.Images.ToList();
                return View(model);
            }


            if (updatedBlog.Photos is not null)
            {
                foreach (var photo in updatedBlog.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(model);
                    }

                    if (!photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }
                }

                foreach (var item in dbBlog.Images)
                {
                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", item.Image);

                    FileHelper.DeleteFile(dbPath);
                }

                List<BlogImage> blogImages = new();

                foreach (var photo in updatedBlog.Photos)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(path, photo);

                    BlogImage blogImage = new()
                    {
                       Image = fileName
                    };

                    blogImages.Add(blogImage);
                }

                await _context.BlogImages.AddRangeAsync(blogImages);
                dbBlog.Images = blogImages;
            }
            else
            {
                Blog blog = new()
                {
                    Images = dbBlog.Images
                };
            }


            dbBlog.Title = updatedBlog.Title;
            dbBlog.Description = updatedBlog.Description; 
            dbBlog.AuthorId= updatedBlog.AuthorId;


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
