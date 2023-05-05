using Microsoft.AspNetCore.Mvc;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        public HomeController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();




            HomeVM model = new()
            {
                Sliders = sliders,
            };

            return View(model);
        }
    }
}
