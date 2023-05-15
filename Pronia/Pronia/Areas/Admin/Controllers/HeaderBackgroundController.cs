using Microsoft.AspNetCore.Mvc;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    public class HeaderBackgroundController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IHeaderBackgroundService _headerBackground;
        public HeaderBackgroundController(AppDbContext context,
                                      IHeaderBackgroundService headerBackground)
        {

            _context = context;
            _headerBackground = headerBackground;
        }
        public IActionResult Index()
        {
            List<HeaderBackground> dbHeaderBackground = _headerBackground.GetHeaderBackgroundsAsync();

            return View(dbHeaderBackground);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            HeaderBackground dbHeaderBackground = await _headerBackground.GetHeaderBackgroundByIdAsync(id);

            HeaderBackground model = new()
            {
                Value = dbHeaderBackground.Value,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, HeaderBackground updatedHeaderBackground)
        {
            try
            {
                if (id == null) return BadRequest();
                HeaderBackground dbHeaderBackground = await _headerBackground.GetHeaderBackgroundByIdAsync(id);
                if (dbHeaderBackground is null) return NotFound();

                HeaderBackground model = new()
                {
                    Value = dbHeaderBackground.Value,
                };

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                dbHeaderBackground.Value = updatedHeaderBackground.Value;
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
                HeaderBackground dbColor = await _headerBackground.GetHeaderBackgroundByIdAsync(id);
                if (dbColor is null) return NotFound();

                _context.HeaderBackgrounds.Remove(dbColor);

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