using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private ICategoryService _categoryService;
        public CategoryController(AppDbContext context,
                                ICategoryService categoryService)

        {
            _context = context;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> dbCategory = await _categoryService.GetCategories();
            return View(dbCategory);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Category dbCategory = await _categoryService.GetByIdAsync(id);
            if (dbCategory is null) return NotFound();
            return View(dbCategory);
        }




        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreatVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }



                Category newCategory = new()

                {
                    Name = model.Name,

                };

                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
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
                Category dbCategory = await _categoryService.GetByIdAsync(id);
                if (dbCategory is null) return NotFound();



                _context.Categories.Remove(dbCategory);

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
            Category dbCategory = await _categoryService.GetByIdAsync(id);
            if (dbCategory is null) return NotFound();

            CategoryUpdateVM model = new()
            {
                Name = dbCategory.Name,
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryUpdateVM categoryUpdate)
        {
            try
            {
                if (id == null) return BadRequest();
                Category dbCategory = await _categoryService.GetByIdAsync(id);
                if (dbCategory is null) return NotFound();

                CategoryUpdateVM model = new()
                {
                    Name = dbCategory.Name,
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }



                dbCategory.Name = categoryUpdate.Name;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }
        }



    }
}
