using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IBannerService _bannerService;
        public BannerController(AppDbContext context, IWebHostEnvironment env, IBannerService bannerService)
        {
            _context = context;
            _env = env;
            _bannerService = bannerService;
   
        }

        public async Task<IActionResult> Index()
        {
            List<Banner> banners = await _bannerService.GetAllAsync();
            return View(banners);
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Banner dbBanner = await _bannerService.GetByIdAsync(id);

            if (dbBanner is null) return NotFound();

            return View(dbBanner);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerCreateVM banner)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(banner);
                }


                if (!banner.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View(banner);
                }

                if (!banner.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View(banner);
                }



                string fileName = Guid.NewGuid().ToString() + "_" + banner.Photo.FileName;
                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                await FileHelper.SaveFileAsync(path, banner.Photo);

                Banner newBanner = new()
                {
                    Image = fileName,
                    Name = banner.Name,
                    Title = banner.Title,
                    IsLarge= banner.IsLarge,
                };

                await _context.Banners.AddAsync(newBanner);
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

                Banner dbBanner = await _bannerService.GetByIdAsync(id);

                if (dbBanner is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbBanner.Image);

                FileHelper.DeleteFile(path);

                _context.Banners.Remove(dbBanner);

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
            if (id is null) return BadRequest();
            Banner dbBanner = await _bannerService.GetByIdAsync(id);
            if (dbBanner is null) return NotFound();

            BannerUpdateVM model = new()
            {
                Image = dbBanner.Image,
                Name = dbBanner.Name,
                Title = dbBanner.Title,
                IsLarge = dbBanner.IsLarge,

            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BannerUpdateVM banner)
        {
            try
            {
                if (id is null) return BadRequest();
                Banner dbBanner = await _bannerService.GetByIdAsync(id);
                if (dbBanner is null) return NotFound();

                BannerUpdateVM model = new()
                {
                    Image = dbBanner.Image,
                    Name = dbBanner.Name,
                    Title = dbBanner.Title,
                    IsLarge = dbBanner.IsLarge,

                };

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (banner.Photo != null)
                {

                    if (!banner.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(banner);
                    }
                    if (!banner.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(banner);
                    }

                    string oldPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbBanner.Image);
                    FileHelper.DeleteFile(oldPath);

                    string fileName = Guid.NewGuid().ToString() + "_" + banner.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(newPath, banner.Photo);

                    dbBanner.Image = fileName;
                }
                else
                {
                    BannerUpdateVM newBanner = new()
                    {
                        Image = dbBanner.Image
                    };
                }


                dbBanner.Name = banner.Name;
                dbBanner.Title = banner.Title;
                dbBanner.IsLarge= banner.IsLarge;


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }


    }
}
