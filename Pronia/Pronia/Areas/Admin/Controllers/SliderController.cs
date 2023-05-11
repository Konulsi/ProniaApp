using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services.Interfaces;
using System;
using System.IO;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISliderService _sliderService;

        public SliderController(AppDbContext context,
                                IWebHostEnvironment env,
                                ISliderService sliderService)
        {
            _context = context;
            _env = env;
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();

            return View(sliders);
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Slider slider = await _sliderService.GetById(id);

            if (slider is null) return NotFound();

            return View(slider);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {
            try
            {
                if (!ModelState.IsValid)  
                {
                    return View(slider);
                }


                if (!slider.Photo.CheckFileType("image/")) 
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                if (!slider.Photo.CheckFileSize(200)) 
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }



                string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;  
                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                await FileHelper.SaveFileAsync(path, slider.Photo);

                Slider newSlider = new()  
                {
                    Image = fileName,
                    Title = slider.Title,
                    Offer = slider.Offer,
                    Description = slider.Description,
                };

                await _context.Sliders.AddAsync(newSlider);
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

                Slider dbSlider = await _sliderService.GetById(id);

                if (dbSlider is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbSlider.Image);

                FileHelper.DeleteFile(path);

                _context.Sliders.Remove(dbSlider);

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
            Slider dbSlider = await _sliderService.GetById(id);
            if (dbSlider is null) return NotFound();

            SliderUpdateVM model = new()
            {
                Image = dbSlider.Image,
                Title = dbSlider.Title,
                Offer = dbSlider.Offer,
                Description = dbSlider.Description,
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderUpdateVM slider)
        {
            try
            {
                if (id is null) return BadRequest();
                Slider dbSlider = await _sliderService.GetById(id);
                if (dbSlider == null) return NotFound();

                SliderUpdateVM model = new()
                {
                    Image = dbSlider.Image,
                    Title = dbSlider.Title,
                    Offer = dbSlider.Offer,
                    Description = dbSlider.Description,
                };

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (slider.Photo != null)
                {

                    if (!slider.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(slider);
                    }
                    if (!slider.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(slider);
                    }

                    string oldPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbSlider.Image);
                    FileHelper.DeleteFile(oldPath);

                    string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(newPath, slider.Photo);

                    dbSlider.Image = fileName;
                }
                else
                {
                    Slider newSlider = new()  
                    {
                        Image = dbSlider.Image
                    };
                }
            

                dbSlider.Title = slider.Title;
                dbSlider.Description = slider.Description;
                dbSlider.Offer = slider.Offer;

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
