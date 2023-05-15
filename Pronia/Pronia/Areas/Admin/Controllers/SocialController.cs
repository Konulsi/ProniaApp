using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SocialController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISocialService _socialService;
        public SocialController(AppDbContext context,
                             ISocialService socialService)
        {

            _context = context;
            _socialService = socialService;
        }
        public async Task<IActionResult> Index()
        {
            List<Social> dbSocials = await _socialService.GetAllSocials();
            return View(dbSocials);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return BadRequest();
            Social dbSocial = await _socialService.GetSocialById(id);
            if (dbSocial == null) return NotFound();

            return View(dbSocial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                Social dbSocial = await _socialService.GetSocialById(id);
                if (dbSocial == null) return NotFound();

                _context.Socials.Remove(dbSocial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                throw;
            }


        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) return BadRequest();
            Social dbSocial = await _socialService.GetSocialById(id);
            if (dbSocial == null) return NotFound();

            SocialUpdateVM model = new()
            {
                Link = dbSocial.Link,
            };
            return View(model);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SocialUpdateVM social)
        {

            try
            {
                if (id == null) return BadRequest();
                Social dbSocial = await _socialService.GetSocialById(id);
                if (dbSocial == null) return NotFound();

                SocialUpdateVM model = new()
                {
                    Link = dbSocial.Link,
                };

                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                dbSocial.Link = social.Link;


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