using Microsoft.AspNetCore.Mvc;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.ViewComponents
{
    public class FooterViewComponent: ViewComponent
    {
        private readonly ILayoutService _layoutService;
        private readonly ISocialService _socialService;

        public FooterViewComponent(ILayoutService layoutService, ISocialService socialService)
        {
            _layoutService = layoutService;
            _socialService = socialService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterVM model = new FooterVM()
            {
                Socials = await _socialService.GetAllSocials(),
                Settings =  _layoutService.GetSettingsData(),
            };
            return await Task.FromResult(View(model));
        }


    }
}
    