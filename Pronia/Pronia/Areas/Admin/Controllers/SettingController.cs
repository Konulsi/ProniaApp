using Microsoft.AspNetCore.Mvc;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISettingService _settingService;
        public SettingController(AppDbContext context,
                                      ISettingService settingService)
        {

            _context = context;
            _settingService = settingService;
        }
        public IActionResult Index()
        {
            List<Settings> settings = _settingService.GetSettingsAsync();

            return View(settings);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            Settings dbSetting = await _settingService.GetSettingByIdAsync(id);


            Settings model = new()
            {
                Value = dbSetting.Value,
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Settings updatedSetting)
        {
            try
            {
                if (id == null) return BadRequest();
                Settings dbSetting = await _settingService.GetSettingByIdAsync(id);
                if (dbSetting is null) return NotFound();

                Settings model = new()
                {
                    Value = dbSetting.Value,
                };

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                dbSetting.Value = updatedSetting.Value;
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
                Settings dbSetting = await _settingService.GetSettingByIdAsync(id);
                if (dbSetting is null) return NotFound();

                _context.Settings.Remove(dbSetting);

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
